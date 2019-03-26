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
            #region Chat server
            var bwChat = new BackgroundWorker();
            bwChat.DoWork += BwChat_DoWork;
            bwChat.RunWorkerCompleted += BwChat_RunWorkerCompleted;
            bwChat.RunWorkerAsync();
            #endregion
            #region Area server
            #endregion
            #region Top server
            try
            {
                server = new TcpListener(IPAddress.Any, Nastaveni.PortTop);
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
                var cw = new ClientWorking(client, "");
                lock (Users) Users.Add(cw);
                var bw = new BackgroundWorker();
                bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                bw.DoWork += Bw_DoWork;
                bw.RunWorkerAsync(cw);
            }
            #endregion
        }

        private static void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            (sender as ClientWorking).DoSomethingWithClient();
        }

        private static void BwChat_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void BwChat_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var server = new TcpListener(IPAddress.Any, Nastaveni.PortChat);
                server.Start();
            }
            catch
            {
                Close("Error start chat server.");
                return;
            }
            while (!Closing)
            {
                try
                {
                    var client = server.AcceptTcpClient();
                    var clientStream = client.GetStream();
                    XmlReader xr = XmlReader.Create(clientStream);

                    try
                    {
                        xr.Read();
                        if(xr.NodeType==XmlNodeType.Element)
                        {
                            xr.Read();
                            var id = int.Parse(xr.Value);
                            xr.Read();
                            var text = xr.Value;
                            var u = Users.Where(x => x.Avatar.AvatarID == id).ToList();
                            var s = u[0].GetChatMsg(text);
                            Console.WriteLine(s);
                            client.Close();
                            //Client validation!!
                            foreach (var user in Users)
                            {
                                TcpClient tc = new TcpClient(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), Nastaveni.PortChat);
                                tc.ReceiveTimeout = 6000;
                                tc.SendTimeout = 6000;
                                XmlWriter xw = XmlWriter.Create(clientStream);
                                xw.WriteString(s);
                                tc.Close();
                            }
                        }
                    }
                    catch { }
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
                catch
                {

                }
            }
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
        public Mnf.MnfAvatar Avatar;
        private string Passw = "";

        public ClientWorking(TcpClient Client, string passw)
        {
            this.Client = Client;
            Client.ReceiveTimeout = 10 * 60 * 1000;
            Client.SendTimeout = 10 * 60 * 1000;
            ClientStream = Client.GetStream();
            Avatar = new Mnf.MnfAvatar();
            Passw = passw;
        }

        public void Init()
        {
        }

        public string GetChatMsg(string msg)
        {
            return "<message id=\"" + Avatar.AvatarID + "\" name=\"" + Avatar.JmenoPostavy + "\" color=\"" + Avatar.userCT + "\" text=\"" + msg + "\"></message>";            
        }

        public void DoSomethingWithClient()
        {
            try
            {
                StreamWriter sw = new StreamWriter(ClientStream) { AutoFlush = true };
                StreamReader sr = new StreamReader(ClientStream);
                XmlReader xr = XmlReader.Create(ClientStream);
                XmlWriter xw = XmlWriter.Create(ClientStream);

                xr.Read();
                if (xr.NodeType==XmlNodeType.Element)
                {
                    xr.Read();
                    Avatar.AvatarID = int.Parse(xr.Value);
                    xr.Read();
                    var passw = xr.Value;
                    if (Passw != passw) throw new Exception("Invalid password!");
                }


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
