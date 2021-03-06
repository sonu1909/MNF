﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mnf
{
    /// <summary>
    /// Interaction logic for MnfLogger.xaml
    /// </summary>
    public partial class MnfLogger : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        ObservableCollection<Uzivatel> _Uzivatele = new ObservableCollection<Uzivatel>();
        public ObservableCollection<Uzivatel> Uzivatele
        {
            get { return _Uzivatele; }
            set
            {
                _Uzivatele = value;
                OnPropertyChanged("Uzivatele");
            }
        }
        public int UzivateleSelected = -1;
        MnfUzivatel mnfUzivatel = new MnfUzivatel();
        ObservableCollection<Avatar> _Avatars = new ObservableCollection<Avatar>();
        public ObservableCollection<Avatar> Avatars
        {
            get { return _Avatars; }
            set
            {
                _Avatars = value;
                OnPropertyChanged("Avatars");
            }
        }
        public int AvatarsSelected = -1;
        MnfAvatar mnfAvatar = new MnfAvatar();
        ObservableCollection<Server> _Servers = new ObservableCollection<Server>();
        public ObservableCollection<Server> Servers
        {
            get { return _Servers; }
            set
            {
                _Servers = value;
                OnPropertyChanged("Servers");
            }
        }
        public int ServersSelected = -1;
        MnfServer mnfServer
        {
            get { return _mnfServer; }
            set
            {
                _mnfServer = value;
                SI.DataContext = value;
                OnPropertyChanged("mnfServer");
            }
        }
        public MnfServer _mnfServer = new MnfServer();

WebClient wc = new WebClient();

        public event EventHandler<MnfPlayer> NewUser;
        private void OnNewUser(MnfPlayer mp)
        {
            NewUser?.Invoke(this, mp);
        }

        public MnfLogger()
        {
            InitializeComponent();
            DataContext = this;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                if (!File.Exists(NastaveniMnf.UserFile)) File.Create(NastaveniMnf.UserFile).Close();
                StreamReader sr = new StreamReader(NastaveniMnf.UserFile);
                var radek = sr.ReadLine();
                while (radek != null)
                {
                    var u = new Uzivatel();
                    u.JmenoUzivatele = radek;
                    u.HesloUzivatele = sr.ReadLine();
                    radek = sr.ReadLine();
                    Uzivatele.Add(u);
                }
                sr.Close();
                if (LB.Items.Count > 0) LB.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nepodařilo se nahrát uživatele\n" + ex.Message);
                //MessageBox.Show("Nepodařilo se nahrát uživatele\n" + ex.Message);
            }
        }
        public void Close()
        {
            StreamWriter sw = new StreamWriter(NastaveniMnf.UserFile, false);
            foreach (var u in Uzivatele)
            {
                sw.WriteLine(u.JmenoUzivatele);
                sw.WriteLine(u.HesloUzivatele);
            }
            sw.Close();
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            MnfLogin ml = new MnfLogin();
            if (ml.ShowDialog().Value)
            {
                var u = new Uzivatel();
                u.JmenoUzivatele = ml.User.JmenoUzivatele;
                u.HesloUzivatele = CalculateMD5Hash(ml.User.JmenoUzivatele.ToLower() + ml.User.HesloUzivatele).ToLower();
                Uzivatele.Add(u);
            }
        }

        private void RemUser(object sender, RoutedEventArgs e)
        {
            Uzivatele.RemoveAt(LB.SelectedIndex);
            if (LB.SelectedIndex > -1) LB.SelectedIndex--;
        }

        private void LB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //MnfGame.Close();
            if (LB.SelectedIndex < 0) return;
            mnfUzivatel = new MnfUzivatel();
            //TODO disconnect logged user!   
            UserLogIn(Uzivatele[LB.SelectedIndex]);
            UzivateleSelected = LB.SelectedIndex;
        }
        public void LB_Select(int i)
        {
            mnfUzivatel = new MnfUzivatel();
            //TODO disconnect logged user!   
            UserLogIn(Uzivatele[i]);
            UzivateleSelected = i;
        }
        public void GetVersion()
        {
            Console.WriteLine("Initizing Version");
            var responseNON = wc.DownloadString("http://www.mnfclub.com/game.html");
            Properties.Settings.Default.Verze = (from f in responseNON.Split('\n') where f.Contains("value=\"http://www.mnfclub.com/swf/main.swf?version=") select f.Split('"')[3].Split('=')[1]).ToArray()[0];
            Properties.Settings.Default.Save();
            Console.WriteLine("Version is " + Properties.Settings.Default.Verze);
            Console.WriteLine("Initizing http servers");
            responseNON = wc.DownloadString(MnfAddress.SiteMain("images/left_couple_purple.jpg"));
            responseNON = wc.DownloadString(MnfAddress.SiteMain("images/right_couple_purple.jpg"));
            responseNON = wc.DownloadString(MnfAddress.SiteMain("bug_report.php"));
            responseNON = wc.DownloadString(MnfAddress.SiteMain("images/send_btn.gif"));
            responseNON = wc.DownloadString(MnfAddress.SiteMain("images/close_btn.gif"));

        }
        /// <summary>
        /// pripoji se a nacte avatary
        /// </summary>
        /// <param name="u"></param>
        public void UserLogIn(Uzivatel u)
        {
            try
            {
                GetVersion();
                Console.WriteLine("Download main.swf");
                var responseNON = wc.DownloadString(MnfAddress.SiteSWF("main.swf?version=" + Properties.Settings.Default.Verze));
                string[] toDown = new string[] { "highscores", "dialog_manager", "outlined_font", "bubble_manager", "chat_manager", "picture_viewer", "avatar_info", "custom_bg_manager", "friends_list", "mail_manager", "invite_manager", "item_manager", "system_message_manager", "game_settings", "login_screen", "emoticons", "petnis", "avatar" };
                foreach (var td in toDown)
                {
                    Console.WriteLine("Download " + td + ".swf");
                    responseNON = wc.DownloadString(MnfAddress.SiteSWF(td + ".swf?version=" + Properties.Settings.Default.Verze));
                }
                string s;
                //Logovani
                var data = new NameValueCollection();
                data["pass"] = u.HesloUzivatele;
                data["email"] = u.JmenoUzivatele;
                mnfUzivatel.LoginPaswCrypted = u.HesloUzivatele;

                Console.WriteLine("Loging to game");
                var response = wc.UploadValues(MnfAddress.SiteMain() + MnfAddress.SiteLogin, "POST", data);//&errors=00&user_id=1658254&premium=0&premium_notification=0&overcrowder=0&
                s = Encoding.UTF8.GetString(response, 0, response.Length);
                if (UserParse(s)) { MessageBox.Show("Bad login\n" + s); return; }
                Console.WriteLine("Loging ok");
                Console.WriteLine("Download bug_report & avatar_manager_screen");
                responseNON = wc.DownloadString(MnfAddress.SiteMain("bug_report.php?email=" + u.JmenoUzivatele + "&avatar="));
                responseNON = wc.DownloadString(MnfAddress.SiteSWF("avatar_manager_screen.swf?1." + Properties.Settings.Default.Verze));
                ////Zisk Avatara
                data = new NameValueCollection();
                data["pass"] = u.HesloUzivatele;
                data["user_id"] = mnfUzivatel.UserID.ToString();//nebo user%5Fid

                Console.WriteLine("read avatars");
                response = wc.UploadValues(MnfAddress.SiteMain() + MnfAddress.SiteAvatar, "POST", data);
                s = Encoding.UTF8.GetString(response, 0, response.Length);
                string[] ss = s.Split('&')[1].Split(';');
                //if (ss.Length < 5) { MessageBox.Show("bad response\n" + s); return true; }
                Avatars.Clear();
                for (int i = 0; i < ss.Length; i++)
                {
                    var a = new Avatar();
                    if (a.ParseAvatar(ss[i])) MessageBox.Show("bad response\n" + ss[i]);
                    else
                    {
                        Avatars.Add(a);
                        Console.WriteLine("Avatar add " + a.JmenoPostavy);
                    }
                }
            }
            catch(Exception e) { Console.WriteLine("Nepovedlo se pripojit uzivatele " + u.JmenoUzivatele); Console.WriteLine(e); }
        }
        private bool UserParse(string s)
        {
            try
            {
                string[] ss = s.Split('&');
                //errors = int.Parse(ss[1].Split('=')[1]);
                mnfUzivatel.UserID = int.Parse(ss[2].Split('=')[1]);
                mnfUzivatel.premium = int.Parse(ss[3].Split('=')[1]);
                mnfUzivatel.premium_notification = int.Parse(ss[4].Split('=')[1]);
                mnfUzivatel.overcrowder = int.Parse(ss[5].Split('=')[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
            return false;
        }

        public bool VyberAvatara()
        {
            try
            {
                string s;
                //kontrola brothel
                Console.WriteLine("Brothel CheckUp");
                var data = new NameValueCollection();
                data["avatar_id"] = mnfAvatar.AvatarID.ToString();
                data["pass"] = mnfUzivatel.LoginPaswCrypted;
                data["user_id"] = mnfUzivatel.UserID.ToString();
                var response = wc.UploadValues(MnfAddress.SiteMain() + MnfAddress.SiteBrothel, "POST", data);
                s = Encoding.UTF8.GetString(response, 0, response.Length);
                string[] ss = s.Split('&');
                if (ss[1].Split('=')[1] != "not_working") { MessageBox.Show("Working in Brothel!!\n" + s); return false; }
                var responseNON = wc.DownloadString(MnfAddress.SiteMain("bug_report.php?email=" + Uzivatele[UzivateleSelected].JmenoUzivatele + "&avatar=" + mnfAvatar.JmenoPostavy)); Console.WriteLine("Downloaded bug_report.php");
            }
            catch(Exception e){ Console.WriteLine("Nepovedlo se pripojit avatara");Console.WriteLine(e); }
            return true;
        }

        public void NactiServery()
        {
            try
            {
                string s;
                ////Zisk Serveru
                var data = new NameValueCollection();
                data["color"] = mnfAvatar.userCT;
                data["avatar_id"] = mnfAvatar.AvatarID.ToString();
                data["pass"] = mnfUzivatel.LoginPaswCrypted;
                data["user_id"] = mnfUzivatel.UserID.ToString();
                Console.WriteLine("read servers");
                var response = wc.UploadValues(MnfAddress.SiteMain() + MnfAddress.SiteServer, "POST", data);

                s = Encoding.UTF8.GetString(response, 0, response.Length);
                string[] ss = s.Replace("<server ", ";").Split(';');
                Servers.Clear();
                for (int i = 1; i < ss.Length; i++)
                {
                    Server ms = new Server();
                    if (ms.ParseServer(ss[i])) { MessageBox.Show("Bad response\n" + s); }
                    Servers.Add(ms);
                    Console.WriteLine("Server add " + ms.JmenoServeru);
                    /*
                foreach (MnfServer a in ms)
                {
                    if (a.Kapacita == 0) lb.Items.Add(a.Jmeno + " 0%");
                    else lb.Items.Add(a.Jmeno + " " + ((a.PocetMuzu + a.PocetZen) * 100.0 / a.Kapacita).ToString("0.0") + "%");
                }
                */
                }
                response = wc.UploadValues(MnfAddress.SiteMain() + MnfAddress.SiteServer, "POST", data);
            }
            catch(Exception e) { Console.WriteLine("Nepovedlo se nacist servery");Console.WriteLine(e); }
        }

        private void LBS_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LBS.SelectedIndex < 0) return;
            mnfServer = new MnfServer(Servers[LBS.SelectedIndex]);
            Console.WriteLine("Selected server " + mnfServer.JmenoServeru);
            ServersSelected = LBS.SelectedIndex;
            if (LBS.SelectedIndex > -1)
            {
                OnNewUser(new MnfPlayer()
                {
                    Avatar = mnfAvatar,
                    Server = mnfServer,
                    Uzivatel = mnfUzivatel
                });
            }
        }
        public MnfPlayer LBS_Select(int i)
        {
            MnfPlayer mp = null;
            if (i < 0) return mp;
            mnfServer = new MnfServer(Servers[i]);
            Console.WriteLine("Selected server " + mnfServer.JmenoServeru);
            ServersSelected = i;
            if (i > -1)
            {
                mp = new MnfPlayer()
                {
                    Avatar = mnfAvatar,
                    Server = mnfServer,
                    Uzivatel = mnfUzivatel
                };
            }
            return mp;
        }
        /// <summary>
        /// Aktivuje avatar a nacte servery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LBA_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LBA.SelectedIndex < 0) return;
            mnfAvatar = new MnfAvatar(Avatars[LBA.SelectedIndex]);
            Console.WriteLine("Selected avatar " + mnfAvatar.JmenoPostavy);
            AvatarsSelected = LBA.SelectedIndex;
            if (VyberAvatara()) NactiServery();
            else Servers.Clear();
        }
        public void LBA_Select(int i)
        {
            if (i < 0) return;
            mnfAvatar = new MnfAvatar(Avatars[i]);
            Console.WriteLine("Selected avatar " + mnfAvatar.JmenoPostavy);
            AvatarsSelected = i;
            if (VyberAvatara()) NactiServery();
            else Servers.Clear();
        }

        private void LB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LB_SelectionChanged(null, null);
        }

        private void LBA_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LBA_SelectionChanged(null, null);
        }

        private void LBS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LBS_SelectionChanged(null, null);
        }
    }
}
