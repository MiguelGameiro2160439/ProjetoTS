using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoTS_Cliente
{
    public partial class FormFicheiros : Form
    {
        public FormFicheiros()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int bufferSize = 20480;
            int bytesRead = 0;
            byte[] buffer = new byte[bufferSize];

            String originalFilePath = "security.jpg";
            String copyfilePath = "copy_security.jpg";
            String logFilePath = "log.txt";

            if (File.Exists(copyfilePath))
            {
                File.Delete(copyfilePath);
            }

            FileStream originalFileStream = new FileStream(originalFilePath, FileMode.Open);
            FileStream copyFileStream = new FileStream(copyfilePath, FileMode.Create);
            StreamWriter logStream = new StreamWriter(logFilePath);

            progressBar1.Maximum = (int)originalFileStream.Length;

            while ((bytesRead = originalFileStream.Read(buffer, 0, bufferSize)) > 0)
            {
                copyFileStream.Write(buffer, 0, bytesRead);
                progressBar1.Increment(bytesRead);
                System.Threading.Thread.Sleep(1000);
            }

            String msg = "File copied [" + originalFileStream.Length + " bytes]";
            logStream.Write(msg);

            logStream.Close();
            originalFileStream.Close();
            copyFileStream.Close();
        }
    }
}
