using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTS_Server
{
    class Server
    {
        private const int PORT = 9999;

        static void Main(string[] args)
        {
            TcpListener tcpListener = null;
            TcpClient tcpClient = null;
            NetworkStream networkStream = null;

            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, PORT);
                tcpListener = new TcpListener(endPoint);

                Console.WriteLine("Starting Server...");

                tcpListener.Start();
                Console.WriteLine("Waiting for connections...");

                tcpClient = tcpListener.AcceptTcpClient();
                Console.WriteLine("Client found.");

                networkStream = tcpClient.GetStream();

                int bytesRead = 0;

                #region Receive String Message
                Console.WriteLine("mensagem");

                int bufferSize = tcpClient.ReceiveBufferSize;
                byte[] buffer = new byte[bufferSize];
                bytesRead = networkStream.Read(buffer, 0, buffer.Length);

                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine("Received: " + msg);

                //send Ack

                byte[] ack = Encoding.UTF8.GetBytes("OK");
                Console.WriteLine("Sending: OK");

                networkStream.Write(ack, 0, ack.Length);

                #endregion
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
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

            //Console.WriteLine("Server")
            Console.ReadKey();
        }
    }
}
