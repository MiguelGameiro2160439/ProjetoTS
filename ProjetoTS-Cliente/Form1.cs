using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoTS_Cliente
{
    public partial class Form1 : Form
    {
        private ProtocolSI protocolsi;
        private RSACryptoServiceProvider rsa;
        private const int PORT = 9999;

        public Form1()
        {
            InitializeComponent();
            protocolsi = new ProtocolSI();
        }

        private void button1_Click(object sender, EventArgs e)
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
