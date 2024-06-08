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
    public partial class Extract : Form
    {
        public Extract()
        {
            InitializeComponent();
            this.textBox1.Text = ExtractCKBPath;
            string updatedPath = ExtractCKBPath.Replace("da2sound16.ckb", "").Trim();

            // Update the label text
            this.label3.Text = "Note: Extracted Sounds are on " + updatedPath;
        }

        public static string ExtractCKBPath = "";
        public static string CurrentWorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Cricket Audio Bank File|*.ckb"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExtractCKBPath = dialog.FileName.ToString();
                this.textBox1.Text = ExtractCKBPath;
                string updatedPath = ExtractCKBPath.Replace("da2sound16.ckb", "").Trim();

                // Update the label text
                this.label3.Text = "Note: Extracted Sounds are on " + updatedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialize the ProcessStartInfo
                ProcessStartInfo startInfo = new ProcessStartInfo();
                string HardcodedProgramPath = @"\Tools\cktool\cktool.exe";
                string MergedPath = CurrentWorkingDirectory + HardcodedProgramPath;

                if (!File.Exists(MergedPath))
                {
                    MessageBox.Show("The tool cktool.exe was not found at the expected path: " + MergedPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                startInfo.FileName = MergedPath;
                startInfo.Arguments = "extract -verbose " + ExtractCKBPath;

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

                // Output the process standard output to a TextBox or any other control
                this.richTextBox1.AppendText("\nProcess Output:\n" + output.ToString());

                // Find and rename .wav files
                FindAndRenameWavFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FindAndRenameWavFiles()
        {
            string updatedPath = ExtractCKBPath.Replace("da2sound16.ckb", "").Trim(); // Replace with the actual path or pass it dynamically
            string prefix = "da2sound16_";
            StringBuilder renamedFiles = new StringBuilder();

            try
            {
                string[] wavFiles = Directory.GetFiles(updatedPath, "*.wav", SearchOption.AllDirectories);
                foreach (string wavFile in wavFiles)
                {
                    string fileName = Path.GetFileName(wavFile);
                    if (fileName.StartsWith(prefix))
                    {
                        string newFileName = fileName.Substring(prefix.Length);
                        string newFilePath = Path.Combine(Path.GetDirectoryName(wavFile), newFileName);

                        File.Move(wavFile, newFilePath);
                        renamedFiles.AppendLine(newFilePath);
                    }
                }

                // Output renamed file paths to a TextBox or any other control
                this.richTextBox1.AppendText("\nRenamed Files:\n" + renamedFiles.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}