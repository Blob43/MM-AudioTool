using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MM_AudioTool
{
    public partial class Complie : Form
    {
        public Complie()
        {
            InitializeComponent();
            this.textBox1.Text = ExtractCKBPath;
            string updatedPath = ExtractCKBXPath.Replace("da2sound16.ckbx", "").Trim();

            // Update the label text
            this.label2.Text = "Will be saved to " + updatedPath;
        }

        public static string ExtractCKBPath = "";
        public static string ExtractCKBXPath = "";
        public static string CurrentWorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Cricket Audio Description File|*.ckbx"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExtractCKBPath = dialog.FileName.ToString();
                this.textBox1.Text = ExtractCKBPath;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Cricket Audio Bank File|*.ckb"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExtractCKBXPath = dialog.FileName.ToString();
                this.textBox2.Text = ExtractCKBXPath;
                string updatedPath = ExtractCKBXPath;

                // Update the label text
                this.label2.Text = "Will be saved to " + updatedPath;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Check if the paths are empty
            if (string.IsNullOrWhiteSpace(ExtractCKBPath) || string.IsNullOrWhiteSpace(ExtractCKBXPath))
            {
                MessageBox.Show("Both paths must be set before starting the process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.richTextBox1.Text = "This may take a while... (May cause to hang a bit)";
            MessageBox.Show("This may take a while... (May cause to hang a bit)");

            // Initialize the ProcessStartInfo
            ProcessStartInfo startInfo = new ProcessStartInfo();
            string HardcodedProgramPath = @"\tools\cktool\cktool.exe";
            string MergedPath = System.IO.Path.Combine(CurrentWorkingDirectory, HardcodedProgramPath);
            startInfo.FileName = MergedPath;
            startInfo.Arguments = "buildbank -verbose " + ExtractCKBPath + " " + ExtractCKBXPath;

            // Redirect the standard output so that we can capture it
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            // Start the process
            Process process = new Process
            {
                StartInfo = startInfo
            };
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

            this.richTextBox1.Text = stdout;
        }

        private void Complie_Load(object sender, EventArgs e)
        {

        }
    }
}
