using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Xml;
using System.ComponentModel;

namespace MnfServer
{
    class Program
    {
        static int MaxUsers = 300;
        static MySqlConnection connection;
        static TcpListener server = null;
        static bool Closing = false;
        static List<ClientWorking> Users = new List<ClientWorking>();
        static int GetNumUsers()
        {
            int i = MaxUsers;
            lock (Users) i = Users.Count;
            return i;
        }
        static void Main(string[] args)
        {
            #region aplication init
            try
            {
                Nastaveni.Init();
            }
            catch
            {
                Close("Error reading Init.");
                return;
            }
            #endregion
            #region database init
            string connstring = string.Format("Server=" + Nastaveni.DBserver + "; database=" + Nastaveni.DBname + "; UID=" + Nastaveni.DBuser + "; password=" + Nastaveni.DBpassw);
            try
            {
                connection = new MySqlConnection(connstring);
                connection.Open();
            }
            catch
            {
                Close("Error connect to DB.");
                return;
            }
            #endregion
            #region
            #endregion
            #region Server start
            try
            {
                server = new TcpListener(IPAddress.Any, Nastaveni.PortIP);
                server.Start();
            }
            catch
            {
                Close("Error start server.");
                return;
            }
            while (!Closing)
            {
                var client = server.AcceptTcpClient();
                if (GetNumUsers() >= MaxUsers) return;
                client.ReceiveTimeout = 10 * 60 * 1000;
                client.SendTimeout = 10 * 60 * 1000;
                var cw = new ClientWorking(client);
                lock (Users) Users.Add(cw);
                var bw = new BackgroundWorker();
                bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                bw.DoWork += Bw_DoWork;
                bw.RunWorkerAsync(cw);
            }
            #endregion
        }

        private static void Bw_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private static void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        static void Close(string s)
        {
            Console.WriteLine(s);
            Console.ReadLine();
            connection?.Close();
            server?.Stop();
            Closing = true;
        }
    }
    public class ClientWorking
    {
        private Stream ClientStream;
        private TcpClient Client;

        public ClientWorking(TcpClient Client)
        {
            this.Client = Client;
            Client.ReceiveTimeout = 10 * 60 * 1000;
            Client.SendTimeout = 10 * 60 * 1000;
            ClientStream = Client.GetStream();
        }

        public void Init()
        {
        }

        public void DoSomethingWithClient()
        {
            try
            {
                StreamWriter sw = new StreamWriter(ClientStream) { AutoFlush = true };
                StreamReader sr = new StreamReader(ClientStream);
                XmlReader xr = XmlReader.Create(ClientStream);
                XmlWriter xw = XmlWriter.Create(ClientStream);



                while (true)
                {
                    while (xr.Read())
                    {
                        switch (xr.NodeType)
                        {
                            case XmlNodeType.Element:
                                switch (xr.Name)
                                {

                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Server was disconnect from client.");
                Console.WriteLine(e.Message);
            }
        }
    }
}
