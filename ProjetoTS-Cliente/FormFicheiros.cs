using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ProjetoTS_Cliente
{
    public partial class FormFicheiros : Form
    {
        private ProtocolSI protocolsi;
        private RSACryptoServiceProvider rsa;
        private const int PORT = 9999;
        private const int SALTSIZE = 8;
        public FormFicheiros()
        {
            InitializeComponent();
            rsa = new RSACryptoServiceProvider();
            TcpClient tcpClient = new TcpClient();
            protocolsi = new ProtocolSI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int bufferSize = 20480;
            int bytesRead = 0;
            byte[] buffer = new byte[bufferSize];

            String originalFilePath = "estg_logo.jpg";
            String copyfilePath = @"C:\Users\migue\Documents\ESTG\Desenvolvimento de Aplicações\ProjetoTS-Cliente\ProjetoTS-Server\bin\Debug\estg_logo.jpg";
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

        private void button2_Click(object sender, EventArgs e)
        {
            rsa = new RSACryptoServiceProvider();
            TcpClient tcpClient = new TcpClient();
            NetworkStream networkStream = null;

            try
            {
                tcpClient = new TcpClient();
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, PORT);

                //Console.WriteLine("Start connection to server? press any key");
                //Console.ReadKey();

                tcpClient.Connect(endPoint);
                networkStream = tcpClient.GetStream();

                int bytesRead = 0;

                networkStream.Read(protocolsi.Buffer, 0, protocolsi.Buffer.Length);

                if (protocolsi.GetCmdType() == ProtocolSICmdType.PUBLIC_KEY)
                {
                    MessageBox.Show("Recebi: " + protocolsi.GetStringFromData());
                }

                #region Send String Message

                // Console.WriteLine("Send message to server? write a message");
                string msg = Console.ReadLine();

                byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
                networkStream.Write(msgBytes, 0, msgBytes.Length);

                // Receive ack

                byte[] ack = new byte[2];

                bytesRead = networkStream.Read(ack, 0, ack.Length);

                rsa.FromXmlString("publickey");

                string ackMessage = Encoding.UTF8.GetString(ack, 0, bytesRead);

                //Console.WriteLine("Received:" + ackMessage);

                #endregion
            }
            catch (Exception exception)
            {
                //Console.WriteLine(exception.Message);
            }

            finally
            {
                if (networkStream != null)
                {
                    networkStream.Close();
                }

                if (tcpClient != null)
                {
                    tcpClient.Close();
                }
            }

            //Console.WriteLine("Server");
            //Console.ReadKey();
        }

    }
    
}
