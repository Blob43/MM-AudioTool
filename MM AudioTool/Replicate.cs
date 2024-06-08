using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MM_AudioTool
{
    public partial class Replicate : Form
    {
        public Replicate()
        {
            InitializeComponent();
            this.textBox1.Text = ExtractCKBPath;
            string updatedPath = ExtractCKBPath.Replace("da2sound16.ckb", "").Trim();

            this.label4.Text = "Replicated .ckb will be saved as " + updatedPath;
        }

        public static string ExtractCKBPath = "";
        public static string CurrentWorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Cricket Audio Bank File|*.ckb";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExtractCKBPath = dialog.FileName.ToString();
                this.textBox1.Text = ExtractCKBPath;
                string updatedPath = ExtractCKBPath.Replace(".ckb", ".ckbx").Trim();
                this.label4.Text = "Replicated .ckb will be saved as " + updatedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Initialize the ProcessStartInfo
            ProcessStartInfo startInfo = new ProcessStartInfo();
            string HardcodedProgramPath = @"\Tools\cktool\cktool.exe";
            string MergedPath = CurrentWorkingDirectory + HardcodedProgramPath;
            startInfo.FileName = MergedPath;
            startInfo.Arguments = "info -verbose " + ExtractCKBPath;

            // Redirect the standard output so that we can capture it
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            // Start the process
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            // Read the standard output into a multiline string
            StringBuilder output = new StringBuilder();
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                output.AppendLine(line);
            }

            // Wait for the process to exit
            process.WaitForExit();

            // Convert the StringBuilder to a string
            string stdout = output.ToString();

            // Define regex patterns to extract the required fields
            string soundPattern = @"sound \d+:([\s\S]*?)(?=sound \d+:|$)";
            string namePattern = @"name:\s*(\S+)";
            string formatPattern = @"format:\s*(\d+)";
            string volumePattern = @"volume:\s*([\d.]+)";
            string panPattern = @"pan:\s*([\d.]+)";
            string loopCountPattern = @"loop count:\s*(\d+)";

            // Match all sound blocks
            var soundMatches = Regex.Matches(stdout, soundPattern);

            // Create the XML document
            XDocument xmlDocument = new XDocument(new XElement("bank", new XAttribute("name", "da2sound")));

            foreach (Match soundMatch in soundMatches)
            {
                string soundBlock = soundMatch.Groups[1].Value;

                string name = Regex.Match(soundBlock, namePattern).Groups[1].Value;
                string format = Regex.Match(soundBlock, formatPattern).Groups[1].Value;
                string volume = Regex.Match(soundBlock, volumePattern).Groups[1].Value;
                string pan = Regex.Match(soundBlock, panPattern).Groups[1].Value;
                string loopCount = Regex.Match(soundBlock, loopCountPattern).Groups[1].Value;

                // Use "pcm16" as the format for all sounds
                string formatValue = "pcm16";

                XElement soundElement = new XElement("sound",
                    new XAttribute("name", name),
                    new XAttribute("source", name),
                    new XAttribute("format", formatValue),
                    new XAttribute("volume", volume),
                    new XAttribute("pan", pan),
                    new XAttribute("loopCount", loopCount));

                xmlDocument.Root.Add(soundElement);
            }

            // Save the XML document to a file
            string xmlOutputPath = "output.xml";
            string updatedPath = ExtractCKBPath.Replace(".ckb", ".ckbx").Trim();
            string ckbxpath = updatedPath;
            xmlDocument.Save(updatedPath);
            this.richTextBox1.Text = "Saved to " + updatedPath.ToString();

        }
    }
}
