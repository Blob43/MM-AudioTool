using System.Diagnostics;

namespace MM_AudioTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Replicate re = new Replicate();
            re.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Extract ex = new Extract();
            ex.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Complie cp = new Complie();
            cp.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.appsomniacs.com/",
                UseShellExecute = true
            });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }
    }
}
