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
    public partial class Form1 : Form
    {
        private ProtocolSI protocolsi;
        private RSACryptoServiceProvider rsa;
        private const int PORT = 9999;
        private const int SALTSIZE = 8;

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

        private bool VerifyLogin(string username, string password)
        {
            SqlConnection conn = null;
            try
            {
                // Configurar ligação à Base de Dados
                conn = new SqlConnection();
                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\migue\Documents\ESTG\Tópicos de Segurança\FichaPratica8_Base\FichaPratica8_Base\Database1.mdf';Integrated Security=True");

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração do comando SQL
                String sql = "SELECT * FROM Users WHERE Username = @username";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;

                // Declaração dos parâmetros do comando SQL
                SqlParameter param = new SqlParameter("@username", username);

                // Introduzir valor ao parâmentro registado no comando SQL
                cmd.Parameters.Add(param);

                // Associar ligação à Base de Dados ao comando a ser executado
                cmd.Connection = conn;

                // Executar comando SQL
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("Error while trying to access an user");
                }

                // Ler resultado da pesquisa
                reader.Read();

                // Obter Hash (password + salt)
                byte[] saltedPasswordHashStored = (byte[])reader["SaltedPasswordHash"];

                // Obter salt
                byte[] saltStored = (byte[])reader["Salt"];

                conn.Close();

                byte[] pass = Encoding.UTF8.GetBytes(password);

                byte[] hash = GenerateSaltedHash(pass, saltStored);

                return saltedPasswordHashStored.SequenceEqual(hash);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred");
                throw e;
            }
        }

        private void Register(string username, byte[] saltedPasswordHash, byte[] salt)
        {
            SqlConnection conn = null;
            try
            {
                // Configurar ligação à Base de Dados
                conn = new SqlConnection();
                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\migue\Documents\ESTG\Tópicos de Segurança\FichaPratica8_Base\FichaPratica8_Base\Database1.mdf';Integrated Security=True");

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração dos parâmetros do comando SQL
                SqlParameter paramUsername = new SqlParameter("@username", username);
                SqlParameter paramPassHash = new SqlParameter("@saltedPasswordHash", saltedPasswordHash);
                SqlParameter paramSalt = new SqlParameter("@salt", salt);

                // Declaração do comando SQL
                String sql = "INSERT INTO Users (Username, SaltedPasswordHash, Salt) VALUES (@username,@saltedPasswordHash,@salt)";

                // Prepara comando SQL para ser executado na Base de Dados
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Introduzir valores aos parâmentros registados no comando SQL
                cmd.Parameters.Add(paramUsername);
                cmd.Parameters.Add(paramPassHash);
                cmd.Parameters.Add(paramSalt);

                // Executar comando SQL
                int lines = cmd.ExecuteNonQuery();

                // Fechar ligação
                conn.Close();
                if (lines == 0)
                {
                    // Se forem devolvidas 0 linhas alteradas então o não foi executado com sucesso
                    throw new Exception("Error while inserting an user");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error while inserting an user:" + e.Message);
            }
        }

        private static byte[] GenerateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return buff;
        }

        private static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            using (HashAlgorithm hashAlgorithm = SHA512.Create())
            {
                // Declarar e inicializar buffer para o texto e salt
                byte[] plainTextWithSaltBytes =
                              new byte[plainText.Length + salt.Length];

                // Copiar texto para buffer
                for (int i = 0; i < plainText.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainText[i];
                }
                // Copiar salt para buffer a seguir ao texto
                for (int i = 0; i < salt.Length; i++)
                {
                    plainTextWithSaltBytes[plainText.Length + i] = salt[i];
                }

                //Devolver hash do text + salt
                return hashAlgorithm.ComputeHash(plainTextWithSaltBytes);
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            String pass = textBoxPassword.Text;

            String username = textBoxUsername.Text;

            byte[] salt = GenerateSalt(SALTSIZE);

            byte[] hash = GenerateSaltedHash(Encoding.UTF8.GetBytes(pass), salt);

            Register(username, hash, salt);

            ClientThread ct = new ClientThread("Thread 1");
            ct.run();

            ct = new ClientThread("Thread 2");
            ct.run();
        }

        private class ClientThread
        {
            private String message;

            public ClientThread(String message)
            {
                this.message = message;
            }

            public void run()
            {
                Thread t = new Thread(handleClient);
                t.Start();
            }

            private void handleClient()
            {
                MessageBox.Show(message);
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            String pass = textBoxPassword.Text;

            byte[] salt = GenerateSalt(SALTSIZE);

            byte[] hash = GenerateSaltedHash(Encoding.UTF8.GetBytes(pass), salt);

            //textBoxSaltedHash.Text = Convert.ToBase64String(hash);

            //textBoxSalt.Text = Convert.ToBase64String(salt);

            String username = textBoxUsername.Text;

            if (VerifyLogin(username, pass))
            {
                MessageBox.Show("Valid User");
                FormFicheiros form = new FormFicheiros();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Invalid User");
            }
        }

    }
}
