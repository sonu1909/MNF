using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MnfPic
{
    public class MnfServer
    {
        //<server name="Beaver Bash" ip="93.190.138.189" males="111" females="114" capacity="220" top_socket="2020" area_socket="2021" game_socket="2022" chat_socket="2023" policy_socket="843" />
        public string Jmeno = "";
        public string AdresaIP = "";
        public int PocetMuzu = 0;
        public int PocetZen = 0;
        public int Kapacita = 0;
        public int top_socket = 0;
        public int area_socket = 0;
        public int game_socket = 0;
        public int chat_socket = 0;
        public int policy_socket = 0;

        public MnfAvatar Avatar;
        DateTime server_time = DateTime.Now;
        string session_id = "";
        
        //string mainFile = "";
        WebClient wc = new WebClient();
        TcpClient TC_top = new TcpClient();
        TcpClient TC_game = new TcpClient();
        TcpClient TC_area = new TcpClient();
        TcpClient TC_chat = new TcpClient();
        MnfArea mnfArea = new MnfArea();

        public MnfServer(MnfAvatar avatar)
        {
            Avatar = avatar;  
        }        

        /// <summary>
        /// name="Beaver Bash" ip="93.190.138.189" males="98" females="100" capacity="200" top_socket="2020" area_socket="2021" game_socket="2022" chat_socket="2023" policy_socket="843" />
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true = error</returns>
        public bool StringParse(string s)
        {
            string[] ss = s.Split('=');
            Jmeno = ss[1].Split('\"')[1];
            AdresaIP = ss[2].Split('\"')[1];
            PocetMuzu = int.Parse(ss[3].Split('\"')[1]);
            PocetZen = int.Parse(ss[4].Split('\"')[1]);
            Kapacita = int.Parse(ss[5].Split('\"')[1]);
            top_socket = int.Parse(ss[6].Split('\"')[1]);
            area_socket = int.Parse(ss[7].Split('\"')[1]);
            game_socket = int.Parse(ss[8].Split('\"')[1]);
            chat_socket = int.Parse(ss[9].Split('\"')[1]);
            policy_socket = int.Parse(ss[10].Split('\"')[1]);

            return false;
        }

        public void AktivujAvatar()
        {
            string s;

            //get policy
            TcpClient TC_policy = new TcpClient();
            TC_policy.Connect(AdresaIP, policy_socket);
            NetworkStream ns = TC_policy.GetStream();
            byte[] b = new byte[65535];
            int i = ns.Read(b, 0, b.Length);
            s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
            TC_policy.Close();

            //aktivace postavy
            var data = new NameValueCollection();
            data["color"] = Avatar.userCT;
            data["avatar_id"] = Avatar.AvatarID.ToString();// _ -> %5
            data["pass"] = Avatar.User.LoginPaswCrypted;
            data["user_id"] = Avatar.User.UserID.ToString();
            var response = wc.UploadValues(MnfAddress.SiteMain + MnfAddress.SiteActive, "POST", data);
            s = Encoding.UTF8.GetString(response, 0, response.Length);
            string[] ss = s.Split('&');
            if (ss[1].Split('=')[1] != "1") { throw new Exception("nelze se pripojit!!\n" + s); }
        }
        Random r = new Random();
        public void VyberServery()
        {
            string s;
            if (!TC_top.Connected) TC_top.Connect(AdresaIP, top_socket);
            s = "<data avatar_id=\"" + Avatar.AvatarID + "\" password=\"" + Avatar.User.LoginPaswCrypted + "\" />";
            NetworkStream ns = TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
            try
            {
                byte[] b = new byte[65535];
                int i = ns.Read(b, 0, b.Length);
                s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);

                string[] ss = s.Split('\"');
                DateTime.TryParse(ss[1], out server_time);//overit funkcnost?
                session_id = ss[3];

                //GoToArea(mnfArea.StartID[r.Next(mnfArea.StartID.Count)]);
                GoToArea(25);
                //bw.RunWorkerAsync();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public bool PoSlozkach = true;
        public bool GoToArea(int area)
        {
            string s;

            if (TC_chat.Connected) TC_chat.Close();
            if (TC_area.Connected) TC_area.Close();

            TC_chat = new TcpClient();
            TC_area = new TcpClient();

            if (!TC_top.Connected) TC_top.Connect(AdresaIP, top_socket);
            if (!TC_area.Connected) TC_area.Connect(AdresaIP, area_socket);
            //if (!TC_game.Connected) TC_game.Connect(AdresaIP, game_socket);
            if (!TC_chat.Connected) TC_chat.Connect(AdresaIP, chat_socket);

            s = "<data area_info=\"" + mnfArea.Jmena[area] + "\" />";
            NetworkStream ns = TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

            s = "";
            s += Avatar.AvatarID + ",";
            s += Avatar.User.LoginPaswCrypted + ",";
            s += mnfArea.GetAreaID(mnfArea.Jmena[area]) + ",";
            s += session_id + ",";
            s += Avatar.userCT;
            ns = TC_chat.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

            s = "<data type=\"area\" avatar_id=\"" + Avatar.AvatarID + "\" password=\"" + Avatar.User.LoginPaswCrypted + "\" area_id=\"" + mnfArea.GetAreaID(mnfArea.Jmena[area]) + "\" points=\"" + (mnfArea.Porty[area].X + r.Next(-5, 5)) + "," + (mnfArea.Porty[area].Y + r.Next(-5, 5)) + "\" bed_ids =\"\" pole_ids=\"\" session_id=\"" + session_id + "\" />";
            LastPoint = mnfArea.Porty[area];
            //socket.send(((((((((((((((("<data type=\"area\" avatar_id=\"" + my_avatar.data.id) + "\" password=\"") + _root.user_pass) + "\" area_id=\"") + _root.area_id) + "\" points=\"") + my_avatar._x) + ",") + my_avatar._y) + "\" bed_ids =\"") + _local3) + "\" pole_ids=\"") + _local4) + "\" session_id=\"") + _root.session_id) + "\" />");
            ns = TC_area.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
            ns.ReadTimeout = 500;
            byte[] b = new byte[65535];
            int i = 0;
            try
            {
              i = ns.Read(b, 0, b.Length);
            }
            catch { return false; }
            if (i > 0)
            {
                s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
                if (s.Contains("pictures"))
                {
                    int stazeno = 0;
                    string[] ss = s.Split('\'');
                    List<string> obrazky = new List<string>();
                    for (int a = 0; a < ss.Length; a++)
                    {
                        if (a % 2 == 1) obrazky.Add(ss[a]);
                    }
                    foreach (string o in obrazky)
                    {
                        if (NastaveniMnfPic.MainFile == "")
                        {
                            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                NastaveniMnfPic.MainFile = fbd.SelectedPath + "\\";
                            }
                        }
                        string oo = NastaveniMnfPic.MainFile + o.Split('/')[5];
                        //<data get_picture_info='bb1828fc60e44e63ea6889a392b23097'/>//port2030
                        //<picture_info poster_name='PaulinaLira' rating='4.3' is_voted='0' />//resp
                        if (PoSlozkach)
                        {
                            s = "<data get_picture_info='" + Path.GetFileNameWithoutExtension(oo) + "'/>";
                            ns = TC_top.GetStream();
                            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

                            ns.ReadTimeout = 500;
                            byte[] bb = new byte[65535];
                            int ii = 0;
                            try
                            {
                                ii = ns.Read(bb, 0, bb.Length);
                            }
                            catch { }
                            if (ii != 0)
                            {
                                s = Encoding.UTF8.GetString(Array.ConvertAll(bb, x => (byte)x), 0, ii);
                                if (s.Contains("poster_name"))
                                {
                                    string[] sss = s.Split('\'');
                                    string d = NastaveniMnfPic.MainFile + sss[1];
                                    if (!Directory.Exists(d)) Directory.CreateDirectory(d);
                                    oo = d + "\\" + o.Split('/')[5];
                                }
                            }
                        }
                        if (!File.Exists(oo))
                        {
                            wc.DownloadFile(new Uri(o.Replace(".jpg", "_.jpg")), oo);
                            stazeno++;
                        }
                    }
                    Console.WriteLine(stazeno);
                }
                //MessageBox.Show(s);
            }
            return true;
        }
        public Point LastPoint;
        public void GoToPoint(Point bod)
        {
            string s;
            if (!TC_area.Connected) TC_area.Connect(AdresaIP, area_socket);
            //<data points="700.96,605.23,948,631" />
            s = "<data points=\"" + LastPoint.X + "," + LastPoint.Y + "," + bod.X + "," + bod.Y + "\" />";
            LastPoint = bod;
            NetworkStream ns = TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
                //"<avatar data=\"1168373,SissySlutEric,2,2,2,8,3,38,38,1,3,2,1,1,6,3,1,5,2,3,1,3,1,7,176/252/200,1217,1217,-1,,0,0,7,38,2,2,40,38,0,1\" points=\"987,1179,987,1181\" /><avatar data=\"2278205,Teranas,1,3,2,8,1,38,1,1,1,3,2,2,6,2,1,1,3,5,3,1,3,9,176/252/208,8120,620,-1,,0,1,1,0,0,3,38,0,0,1\" points=\"1130,721,1183,723\" /><avatar data=\"3898264,wolfkidd,1,3,1,7,2,35,9,2,2,1,2,2,6,3,1,5,3,1,1,1,1,3,239/176/252,5609,4859,-1,,0,0,11,35,9,3,38,1,0,1\" points=\"764,735,884,676\" /><avatar data=\"2515041,Casslut,2,2,5,5,0,37,37,1,1,3,1,2,7,1,1,4,3,2,1,3,3,7,202/176/252,2771,2021,-1,,0,0,1,0,0,1,0,0,0,1\" points=\"928,691\" />\0"
                //"<avatar_out id=\"2515041\" />\0"
                //"<avatar data=\"3969460,AmandaLove,2,1,1,2,1,24,37,3,3,2,1,2,2,3,1,5,1,4,2,3,4,2,228/176/252,1789,1039,-1,,0,0,10,37,32,1,0,0,0,1\" points=\"1071,689\" />\0<avatar_out id=\"3969460\" />\0<avatar_out id=\"3898264\" />\0<avatar data=\"99742,VzoreCZEk,1,2,2,8,1,38,39,2,2,1,1,2,6,3,1,4,3,1,3,1,1,10,177/176/252,11649,4149,38,GirlsTrueLove,0,1,13,38,39,1,0,0,0,1\" points=\"1092,726\" />\0<avatar id=\"99742\" points=\"1092,726,1354.3,701.65,1354.3,701.65,1337,506\" />\0<avatar id=\"99742\" points=\"1338.64,524.6,1315.3,469.1,1315.3,469.1,1186,333\" />\0<avatar id=\"99742\" points=\"1260.32,411.23,949,247\" />\0<avatar_out id=\"99742\" />\0"
                //"<avatar id=\"1168373\" points=\"987,1181,986,1139\" />\0<avatar id=\"1168373\" points=\"986,1139,989,1121\" />\0<avatar data=\"3946816,Scumbag,1,1,9,2,4,40,1,1,2,1,1,1,5,3,1,3,1,2,3,1,1,8,252/176/194,4569,3819,-1,,0,0,1,0,0,1,0,0,0,1\" points=\"1664,680\" />\0<avatar data=\"2869957,Zephora,2,2,6,5,2,38,40,1,2,2,1,2,3,3,1,4,2,3,1,3,4,3,252/176/178,8269,7519,-1,,0,0,7,38,40,2,36,33,0,1\" points=\"1161,712\" />\0<avatar id=\"3946816\" points=\"1664,680,1197,802\" />\0<avatar id=\"3946816\" points=\"1494.75,724.22,1197,843\" />\0<avatar id=\"2869957\" points=\"1161,712,1064,692\" />\0<avatar id=\"2869957\" points=\"1064,692,972,680\" />\0<avatar id=\"2869957\" points=\"972,680,889,670\" />\0<avatar id=\"3946816\" points=\"1197,843,1208,1017\" />\0<avatar id=\"2869957\" points=\"889,670,815,696\" />\0<avatar id=\"2869957\" points=\"815,696,746,735\" />\0<avatar id=\"3946816\" points=\"1208,1017,1209,1190\" />\0"
    }
}
