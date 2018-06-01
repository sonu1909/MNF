﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml;

namespace MnfPic
{
    /// <summary>
    /// Interaction logic for MnfGame.xaml
    /// </summary>
    public partial class MnfGame : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MnfGame()
        {
            InitializeComponent();
            DataContext = this;
            foreach (var v in MnfArea.Lokace)
            {
                comboBoxArea.Items.Add(v.JmenoLokace);
            }
            if (!File.Exists("MeetPPL.txt")) File.Create("MeetPPL.txt").Close();
            if (!File.Exists("ChatPPL.txt")) File.Create("ChatPPL.txt").Close();
            try
            {
                StreamReader sr = new StreamReader("MeetPPL.txt");
                string radek = sr.ReadLine();
                while (radek != null)
                {
                    var s = radek.Split(' ');
                    MnfAvatar a = new MnfAvatar();
                    try
                    {
                        a.AvatarID = int.Parse(s[0]);
                        a.JmenoPostavy = s[1];
                    }
                    catch { }
                    //neuplne info o postave
                    PotkanePostavy.Add(a);
                    radek = sr.ReadLine();
                }
                sr.Close();
                sr = new StreamReader("ChatPPL.txt");
                radek = sr.ReadLine();
                while (radek != null)
                {
                    var s = radek.Split(' ');
                    MnfAvatar a = new MnfAvatar();
                    try
                    {
                        a.AvatarID = int.Parse(s[0]);
                        a.JmenoPostavy = s[1];
                    }
                    catch { }
                    //neuplne info o postave
                    ChatPostavy.Add(a);
                    radek = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e) { Console.WriteLine("Load file error"); Console.WriteLine(e.Message); }
            
            TopBW.WorkerSupportsCancellation = true;
            TopBW.DoWork += TopBW_DoWork;
            TopBW.RunWorkerCompleted += TopBW_RunWorkerCompleted;

            AreaBW.WorkerSupportsCancellation = true;
            AreaBW.DoWork += AreaBW_DoWork;
            AreaBW.RunWorkerCompleted += AreaBW_RunWorkerCompleted;

            ChatBW.WorkerSupportsCancellation = true;
            ChatBW.DoWork += ChatBW_DoWork;
            ChatBW.RunWorkerCompleted += ChatBW_RunWorkerCompleted;

            GameBW.WorkerSupportsCancellation = true;
            GameBW.DoWork += GameBW_DoWork;
            GameBW.RunWorkerCompleted += GameBW_RunWorkerCompleted;


            PictureBW.WorkerSupportsCancellation = true;
            PictureBW.DoWork += PictureBW_DoWork;
            //PictureBW.RunWorkerCompleted += GameBW_RunWorkerCompleted;

            GamesBW.WorkerSupportsCancellation = true;
            GamesBW.DoWork += GamesBW_DoWork;
            GamesBW.RunWorkerCompleted += GamesBW_RunWorkerCompleted;
        }
        
        Stopwatch sw = new Stopwatch();
        Random r = new Random();
        private MnfPlayer _MP;
        public MnfPlayer MP
        {
            get { return _MP; }
            set { _MP = value; OnPropertyChanged("MP"); }
        }
        private MnfLocation _ActualArea;
        public MnfLocation ActualArea
        {
            get { return _ActualArea; }
            set
            {
                if (value != _ActualArea) { _ActualArea = value; OnPropertyChanged("ActualArea"); }
            }
        }
        MnfAvatar ActivChatAvatar;
        MnfAvatar _InfoAvatar;
        public MnfAvatar InfoAvatar
        {
            get { return _InfoAvatar; }
            set
            {
                if (value != _InfoAvatar) { _InfoAvatar = value; OnPropertyChanged("InfoAvatar"); }
            }
        }
        WebClient wc = new WebClient();
        private ObservableCollection<MnfAvatar> _PotkanePostavy = new ObservableCollection<MnfAvatar>();
        public ObservableCollection<MnfAvatar> PotkanePostavy
        {
            get { return _PotkanePostavy; }
            set { _PotkanePostavy = value; OnPropertyChanged("PotkanePostavy"); }
        }
        private ObservableCollection<MnfAvatar> _AktualniPostavy = new ObservableCollection<MnfAvatar>();
        public ObservableCollection<MnfAvatar> AktualniPostavy
        {
            get { return _AktualniPostavy; }
            set { _AktualniPostavy = value; OnPropertyChanged("AktualniPostavy"); }
        }
        private ObservableCollection<MnfAvatar> _ChatPostavy = new ObservableCollection<MnfAvatar>();
        public ObservableCollection<MnfAvatar> ChatPostavy
        {
            get { return _ChatPostavy; }
            set { _ChatPostavy = value; OnPropertyChanged("ChatPostavy"); }
        }
        public object ChatPostavyLck = new object();
        private int _GameID = 2;
        public int GameID
        {
            get { return _GameID; }
            set { if (_GameID != value) { _GameID = value; OnPropertyChanged("GameID"); } }
        }
        private int _AvatarMoney = 0;
        public int AvatarMoney
        {
            get { return _AvatarMoney; }
            set { if (_AvatarMoney != value) { _AvatarMoney = value; OnPropertyChanged("AvatarMoney"); } }
        }

        private bool _GameRepeat = true;
        public bool GameRepeat
        {
            get { return _GameRepeat; }
            set { if (_GameRepeat != value) { _GameRepeat = value; OnPropertyChanged("GameRepeat"); } }
        }

        private bool _SavePicture = false;
        public bool SavePicture
        {
            get { return _SavePicture; }
            set { if (_SavePicture != value) { _SavePicture = value; OnPropertyChanged("SavePicture"); } }
        }
        private string _ActualPicture = "";
        public string ActualPicture
        {
            get { return _ActualPicture; }
            set { if (_ActualPicture != value) { _ActualPicture = value; OnPropertyChanged("ActualPicture"); } }
        }
        private int _SavedPictures = 0;
        public int SavedPictures
        {
            get { return _SavedPictures; }
            set { if (_SavedPictures != value) { _SavedPictures = value; OnPropertyChanged("SavedPictures"); } }
        }
        BackgroundWorker _GameBW = new BackgroundWorker();
        public BackgroundWorker GameBW { get { return _GameBW; } private set { _GameBW = value; OnPropertyChanged("GameBW"); } }
        BackgroundWorker _PictureBW = new BackgroundWorker();
        public BackgroundWorker PictureBW { get { return _PictureBW; } private set { _PictureBW = value; OnPropertyChanged("PictureBW"); } }
        BackgroundWorker _TopBW = new BackgroundWorker();
        public BackgroundWorker TopBW { get { return _TopBW; } private set { _TopBW = value; OnPropertyChanged("TopBW"); } }
        BackgroundWorker _ChatBW = new BackgroundWorker();
        public BackgroundWorker ChatBW { get { return _ChatBW; } private set { _ChatBW = value; OnPropertyChanged("ChatBW"); } }
        BackgroundWorker _AreaBW = new BackgroundWorker();
        public BackgroundWorker AreaBW { get { return _AreaBW; } private set { _AreaBW = value; OnPropertyChanged("AreaBW"); } }
        BackgroundWorker _GamesBW = new BackgroundWorker();
        public BackgroundWorker GamesBW { get { return _GamesBW; } private set { _GamesBW = value; OnPropertyChanged("GamesBW"); } }

        XmlReaderSettings settings = new XmlReaderSettings
        {
            ConformanceLevel = ConformanceLevel.Fragment,
            CheckCharacters = false,
        };
        public void Init(MnfPlayer mp)
        {
            MP = mp;
            try
            {
                //get policy
                //mp.Server.TC_policy = new TcpClient();
                lock (MP.Server.LockerPolicy)
                    mp.Server.TC_policy.Connect(mp.Server.AdresaIP, mp.Server.policy_socket);
                NetworkStream ns = mp.Server.TC_policy.GetStream();
                byte[] b = new byte[65535];
                int i = ns.Read(b, 0, b.Length);
                var s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
                //Console.WriteLine("Policy: " + s);
                mp.Server.TC_policy.Close();
                mp.Server.TC_policy = new TcpClient();

                if (!TopBW.IsBusy) TopBW.RunWorkerAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Policy error: " + e.Message);
            }
        }

        public void Close()
        {
            StreamWriter sr = new StreamWriter("MeetPPL.txt");
            foreach(var v in PotkanePostavy)
            {
                sr.WriteLine(v.AvatarID + " " + v.JmenoPostavy);
            }
            sr.Close();
            sr = new StreamWriter("ChatPPL.txt");
            foreach (var v in ChatPostavy)
            {
                sr.WriteLine(v.AvatarID + " " + v.JmenoPostavy);
            }
            sr.Close();

            TopBW.CancelAsync();
            AreaBW.CancelAsync();
            ChatBW.CancelAsync();
            GameBW.CancelAsync();
            PictureBW.CancelAsync();
            GamesBW.CancelAsync();

            sw.Stop();
            Console.WriteLine("Elapsed time " + sw.Elapsed);
        }

        public DateTime GetActualServerTime()
        {
            return MP.Server.Server_time.AddMilliseconds(sw.ElapsedMilliseconds);
        }

        private void ChatBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("ChatBW_RunWorkerCompleted");
        }

        private void ChatBW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //lock (MP.Server.LockerChat)
                //    if (!MP.Server.TC_chat.Connected) MP.Server.TC_chat.Connect(MP.Server.AdresaIP, MP.Server.chat_socket);
                //var s = MP.Avatar.icon_id + "," + MP.Uzivatel.LoginPaswCrypted + "," + ActualArea.IdLokace + "," + MP.Server.Session_id + "," + MP.Avatar.userCT;
                //private s+= ",l";
                //ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                while (!ChatBW.CancellationPending)
                {
                    try
                    {
                        NetworkStream ns = MP.Server.TC_chat.GetStream();
                        ns.ReadTimeout = 1000;
                        var b = new byte[65535];
                        var i = ns.Read(b, 0, b.Length);
                        var s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
                        if (s != "") Console.WriteLine("C: " + s);
                        var ss = s.Replace("\0", "").Replace("<", "").Split('>');
                        foreach (var v in ss)
                        {
                            if (v != "")
                            {
                                string[] vv = v.Split(' ');
                                switch (vv[0])
                                {
                                    case "message":
                                        break;
                                    case "emoticon":
                                        break;
                                    case "avatar_enter":
                                        break;
                                    case "avatar_exit":
                                        break;
                                    case "avatar_list":
                                        //avatar
                                        break;
                                    default:
                                        Console.WriteLine("C: Unknowen name " + s);
                                        break;
                                }
                            }
                        }
                    }
                    catch (IOException te) { }
                    catch (Exception ex) { Console.WriteLine("C: " + ex.Message); }
                }
                e.Cancel = true;
            }
            catch (Exception ex) { Console.WriteLine("C: " + ex.Message); }
        }

        private void AreaBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("AreaBW_RunWorkerCompleted");
        }

        private void AreaBW_DoWork(object sender, DoWorkEventArgs e)
        {
            XmlReader XR = null;
            try
            {
                //lock (MP.Server.LockerArea)
                //    if (!MP.Server.TC_area.Connected) MP.Server.TC_area.Connect(MP.Server.AdresaIP, MP.Server.area_socket);
                //Random r = new Random();
                //var s = "<data type=\"area\" avatar_id=\"" + MP.Avatar.AvatarID + "\" password=\"" + MP.Uzivatel.LoginPaswCrypted + "\" area_id=\"" + ActualArea.IdLokace + "\" points=\"" + (ActualArea.PortLokace.X + r.Next(-5, 5)) + "," + (ActualArea.PortLokace.Y + r.Next(-5, 5)) + "\" bed_ids =\"\" pole_ids=\"\" session_id=\"" + MP.Server.Session_id + "\" />";

                //ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

                while (!AreaBW.CancellationPending)
                {
                    lock (MP.Server.LockerArea) if (!MP.Server.TC_area.Connected) throw new Exception("TC_area is disconnected");
                    try
                    {
                        NetworkStream ns = MP.Server.TC_area.GetStream();
                        ns.ReadTimeout = 1000;
                        var b = new byte[65535];
                        var i = ns.Read(b, 0, b.Length);
                        var s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
                        if (s != "") Console.WriteLine("A: " + s);
                        var ss = s.Replace("\0", "").Replace("<", "").Split('>');
                        foreach (var v in ss)
                        {
                            if (v != "")
                            {
                                string[] vv = v.Split(' ');
                                switch (vv[0])
                                {
                                    //<avatar id="5738678" points="576,479,553,441" />
                                    //<avatar data="2427633,elizabeth_h,2,2,1,8,0,2,2,1,3,2,2,2,2,2,1,2,2,2,2,3,3,7,252/176/185,14135,13385,-1,,0,0,11,2,2,2,40,38,0,1" points="503,384,506,387" />
                                    case "avatar":
                                        XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                        XR.Read();
                                        var data = XR.GetAttribute("data");
                                        var points = XR.GetAttribute("points");
                                        if (data != null)
                                        {
                                            var avatar = new MnfAvatar();
                                            avatar.ParseAvatar(data);
                                            var aap = (from f in PotkanePostavy where f.AvatarID == avatar.AvatarID select f).ToArray();
                                            if (aap.Length == 0)
                                            {
                                                Dispatcher.BeginInvoke(new Action(() =>
                                                {
                                                    PotkanePostavy.Add(avatar);
                                                    if (!AktualniPostavy.Contains(avatar)) AktualniPostavy.Add(avatar);
                                                }
                                                ));

                                            }
                                            else if (!AktualniPostavy.Contains(aap[0])) Dispatcher.BeginInvoke(new Action(() =>
                                            {
                                                AktualniPostavy.Add(aap[0]);
                                            }
                                               ));
                                        }
                                        else
                                        {
                                            //avatar moved
                                            var id = XR.GetAttribute("id");
                                        }
                                        break;
                                    case "avatar_out":
                                        //<avatar_out id="3586824" />
                                        XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                        XR.Read();
                                        var avatar_id = long.Parse(XR.GetAttribute("id"));
                                        var ap = (from f in AktualniPostavy where f.AvatarID == avatar_id select f).ToArray();
                                        foreach (var a in ap) if (AktualniPostavy.Contains(a)) Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            AktualniPostavy.Remove(a);
                                        }));
                                        break;
                                    case "pictures":
                                        //XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                        //XR.Read();
                                        break;
                                    case "picture":
                                        if (SavePicture)
                                        {
                                            SpinWait.SpinUntil(() => ActualPicture == "", 1000);
                                            XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                            XR.Read();
                                            if (Directory.Exists(NastaveniMnfPic.MainFile))
                                            {
                                                ActualPicture = XR.GetAttribute("url");
                                                string oo = NastaveniMnfPic.MainFile + ActualPicture.Split('/')[5];
                                                //<data get_picture_info='bb1828fc60e44e63ea6889a392b23097'/>//port2030
                                                //<picture_info poster_name='PaulinaLira' rating='4.3' is_voted='0' />//resp 
                                                //lock (MP.Server.LockerTop)
                                                //    if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
                                                s = "<data get_picture_info='" + Path.GetFileNameWithoutExtension(oo) + "'/>";
                                                ns = MP.Server.TC_top.GetStream();
                                                ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                                            }
                                        }
                                        break;
                                    case "/pictures":
                                        SavePicture = false;
                                        break;
                                    default:
                                        Console.WriteLine("A: Unknowen name" + XR.Name);
                                        break;
                                }
                            }
                        }
                    }
                    catch (IOException te) { }
                    catch (Exception ex) { Console.WriteLine("A: " + ex.Message); }
                }
                e.Cancel = true;
            }
            catch (Exception ex) { Console.WriteLine("A: " + ex.Message); }
        }

        private void TopBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("TopBW_RunWorkerCompleted");
        }

        private void TopBW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //aktivace postavy
                var data = new NameValueCollection();
                data["color"] = MP.Avatar.userCT;
                data["avatar_id"] = MP.Avatar.AvatarID.ToString();// _ -> %5
                data["pass"] = MP.Uzivatel.LoginPaswCrypted;
                data["user_id"] = MP.Uzivatel.UserID.ToString();
                var response = wc.UploadValues(MnfAddress.SiteMain + MnfAddress.SiteActive, "POST", data);
                string s = Encoding.UTF8.GetString(response, 0, response.Length);
                string[] ss = s.Split('&');
                if (ss[1].Split('=')[1] != "1") { throw new Exception("nelze se pripojit!!\n" + s); }

                //lock (MP.Server.LockerTop)
                //    if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
                //s = "<data avatar_id=\"" + MP.Avatar.AvatarID + "\" password=\"" + MP.Uzivatel.LoginPaswCrypted + "\" />";
                //NetworkStream ns = MP.Server.TC_top.GetStream();
                //ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

            }
            catch (Exception ex) { Console.WriteLine("T:" + ex.Message); }
            while (!TopBW.CancellationPending)
            {
                try
                {
                    lock (MP.Server.LockerTop)
                        if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
                    var s = "<data avatar_id=\"" + MP.Avatar.AvatarID + "\" password=\"" + MP.Uzivatel.LoginPaswCrypted + "\" />";
                    NetworkStream ns = MP.Server.TC_top.GetStream();
                    ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                    ns.ReadTimeout = 500;
                    //XmlReader XR = XmlReader.Create(ns, settings);
                    ns = MP.Server.TC_top.GetStream();
                    byte[] b = new byte[16777216];
                    int i = ns.Read(b, 0, b.Length);
                    s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
                    if (s != "") Console.WriteLine("T: " + s);
                    var ss = s.Replace("\0", "").Replace("<", "").Split('>');
                    XmlReader XR = null;
                    foreach (var v in ss)
                    //while (XR.Read())
                    {
                        if (v != "")
                        {
                            string[] vv = v.Split(' ');
                            switch (vv[0])
                            {
                                case "base_data":
                                    XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                    XR.Read();
                                    MP.Server.Server_time = DateTime.FromBinary(long.Parse(XR.GetAttribute("server_time")));
                                    sw.Restart();
                                    Console.WriteLine("Server_time: " + MP.Server.Server_time);
                                    MP.Server.Session_id = XR.GetAttribute("session_id");
                                    Console.WriteLine("Session_id: " + MP.Server.Session_id);
                                    XR.GetAttribute("ignore_ids");
                                    XR.GetAttribute("avatar_data");
                                    //Random r = new Random();
                                    GoToArea(MnfArea.Lokace[MnfArea.StartID[r.Next(MnfArea.StartID.Count - 1)]]);
                                    //XR.Read();
                                    break;
                                //case "unread_msgs_data":
                                //    XR.GetAttribute("avatar_id");
                                //    XR.GetAttribute("msgs_num");
                                //    break;
                                case "friends_list":
                                case "friends_page":
                                    StreamWriter swfl = new StreamWriter("FriendList.txt");
                                    //XR.Read();
                                    //while (XR.Name == "avatar")
                                    //{
                                    //    swfl.WriteLine(XR.GetAttribute("data"));
                                    //    XR.Read();
                                    //}
                                    swfl.Close();
                                    break;
                                case "invite":
                                    break;
                                case "invite_reply":
                                    break;
                                case "invite_canceled":
                                    break;
                                case "avatar_details":
                                    //avatar_details data="99742,VzoreCZEk,1,2,2,8,1,38,1,2,2,1,1,2,6,3,1,4,3,1,3,1,1,9,227/252/176,18686,2686,38,GirlsTrueLove,0,1,1,0,0,3,38,38,0,1" friend="false" status="offline" info="" is_ignoring_you="0" cash="0" dont_disturb="0" from="Czech Republic" about="I like sEXP! and chat                              
                                    XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(s)), settings);
                                    XR.Read();
                                    MnfAvatar ma = new MnfAvatar();
                                    ma.ParseAvatar(XR.GetAttribute("data"));
                                    XR.GetAttribute("friend");
                                    XR.GetAttribute("status");
                                    XR.GetAttribute("info");
                                    XR.GetAttribute("is_ignoring_you");
                                    XR.GetAttribute("cash");
                                    XR.GetAttribute("dont_disturb");
                                    XR.GetAttribute("from");
                                    XR.GetAttribute("about");

                                    for (int p = 0; p < PotkanePostavy.Count; p++)
                                    {
                                        if (PotkanePostavy[p].AvatarID == ma.AvatarID)
                                        {
                                            PotkanePostavy[p].ParseAvatar(XR.GetAttribute("data"));
                                            PotkanePostavy[p].Popis = XR.GetAttribute("about");
                                            break;
                                        }
                                    }
                                    break;
                                case "msg":
                                    //<private_msg id="1152019023" id_from="2844033" id_to="99742" name_from="YourSexDreams" name_to="VzoreCZEk" date_time="09/23/17 8:54 am" read="0" type="" text="ahojik" />
                                    XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                    XR.Read();
                                    var msgID = XR.GetAttribute("id");
                                    ma = new MnfAvatar()
                                    {
                                        AvatarID = int.Parse(XR.GetAttribute("id_from")),
                                        JmenoPostavy = XR.GetAttribute("name_from")
                                    };
                                    MnfAvatar maTo = new MnfAvatar()
                                    {
                                        AvatarID = int.Parse(XR.GetAttribute("id_to")),
                                        JmenoPostavy = XR.GetAttribute("name_to")
                                    };
                                    XR.GetAttribute("date_time");
                                    XR.GetAttribute("read");
                                    if (ma.AvatarID == MP.Avatar.AvatarID)
                                    {
                                        string text = XR.GetAttribute("text");
                                        if (ActivChatAvatar != null)
                                            if (ActivChatAvatar.AvatarID == maTo.AvatarID)
                                            {
                                                spHistory.Dispatcher.BeginInvoke((Action)(() =>
                                                {
                                                    spHistory.Children.Add(new TextBox()
                                                    {
                                                        Text = text,
                                                        IsReadOnly = true,
                                                        HorizontalAlignment = HorizontalAlignment.Right,
                                                        Background = Brushes.LightBlue,
                                                        AcceptsReturn = true,
                                                        TextWrapping = TextWrapping.Wrap,
                                                    });
                                                    svHistory.ScrollToBottom();
                                                }));
                                                //DeliveryApprove(MP.Avatar.AvatarID);
                                            }
                                    }
                                    else
                                    {
                                        if ((from f in PotkanePostavy where f.AvatarID == ma.AvatarID select f).Count() < 1) Dispatcher.BeginInvoke((Action)(() => { PotkanePostavy.Add(ma); }));
                                        string types = XR.GetAttribute("type");
                                        switch (types)
                                        {
                                            case "":
                                                string text = XR.GetAttribute("text");
                                                if (ActivChatAvatar != null)
                                                    if (ActivChatAvatar.AvatarID == ma.AvatarID)
                                                    {
                                                        spHistory.Dispatcher.BeginInvoke((Action)(() =>
                                                        {
                                                            spHistory.Children.Add(new TextBox()
                                                            {
                                                                Text = text,
                                                                IsReadOnly = true,
                                                                HorizontalAlignment = HorizontalAlignment.Left,
                                                                Background = Brushes.LightPink,
                                                                AcceptsReturn = true,
                                                                TextWrapping = TextWrapping.Wrap,
                                                            });
                                                            svHistory.ScrollToBottom();
                                                        }));
                                                        //DeliveryApprove(MP.Avatar.AvatarID);
                                                    }
                                                break;
                                        }
                                    }
                                    DeliveryApprove(ma.AvatarID);
                                    break;
                                case "private_messages":
                                //break;
                                case "private_msg":
                                    //T: <private_msg id="1151074399" id_from="8946383" id_to="2844033" name_from="Tibik" name_to="YourSexDreams" date_time="09/22/17 9:19 pm" read="0" type="" text="hi" />
                                    XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                    XR.Read();
                                    if (XR.AttributeCount > 0)
                                    {
                                        msgID = XR.GetAttribute("id");
                                        ma = new MnfAvatar()
                                        {
                                            AvatarID = int.Parse(XR.GetAttribute("id_from")),
                                            JmenoPostavy = XR.GetAttribute("name_from")
                                        };
                                        if (ma.AvatarID == MP.Avatar.AvatarID) break;
                                        if ((from f in PotkanePostavy where f.AvatarID == ma.AvatarID select f).Count() < 1) Dispatcher.BeginInvoke((Action)(() => { PotkanePostavy.Add(ma); }));
                                        var type = XR.GetAttribute("type");
                                        switch (type)
                                        {
                                            case "":
                                                string text = XR.GetAttribute("text");
                                                LogMsg(ma, text, true);
                                                if (ActivChatAvatar != null)
                                                    if (ActivChatAvatar.AvatarID == ma.AvatarID)
                                                    {
                                                        spHistory.Dispatcher.BeginInvoke((Action)(() =>
                                                        {
                                                            spHistory.Children.Add(new TextBox()
                                                            {
                                                                Text = text,
                                                                IsReadOnly = true,
                                                                HorizontalAlignment = HorizontalAlignment.Left,
                                                                Background = Brushes.LightPink,
                                                                AcceptsReturn = true,
                                                                TextWrapping = TextWrapping.Wrap,
                                                            });
                                                            svHistory.ScrollToBottom();
                                                        }));
                                                        DeliveryApprove(ma.AvatarID);
                                                    }
                                                break;
                                        }
                                    }
                                    break;
                                case "unread_msgs_data":
                                    XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                    XR.Read();
                                    var avatarID = int.Parse(XR.GetAttribute("avatar_id"));
                                    var msgN = int.Parse(XR.GetAttribute("msgs_num"));
                                    if (avatarID != MP.Avatar.AvatarID)
                                        if ((from f in PotkanePostavy where f.AvatarID == avatarID select f).Count() < 1) Dispatcher.BeginInvoke((Action)(() =>
                                        {
                                            PotkanePostavy.Add(new MnfAvatar()
                                            {
                                                AvatarID = avatarID,
                                                JmenoPostavy = "NewAvatar",
                                            });
                                        }));
                                    break;
                                case "delivery_approve":
                                    //messages_history.markAllBubblesRead
                                    break;
                                case "mail_contacts":
                                    break;
                                case "outfit_list":
                                case "outfit_datas":
                                case "outfit_purchase":
                                    break;
                                case "wild_west_motel":
                                case "hotel_plaza":
                                    break;
                                case "items_list":
                                    //<items_list></items_list>
                                    //XR.Read();
                                    break;
                                case "show_items_list":
                                    break;
                                case "clean_area":
                                    //< clean_area area_id = "2000500000" />
                                    break;
                                case "boot_guests":
                                    // < boot_guests area_id = "6407614727" />
                                    break;
                                case "sex_poses_respond":
                                    break;
                                case "brothel_list":
                                    break;
                                case "cash_display":
                                    Console.WriteLine(s);
                                    XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                    XR.Read();
                                    AvatarMoney = int.Parse(XR.GetAttribute("value"));
                                    break;
                                case "exp_display":
                                    break;
                                case "brothel_payment":
                                    break;
                                case "price_list":
                                    break;
                                case "hair_salon_service":
                                    break;
                                case "plastic_surgery_service":
                                    break;
                                case "acquire_sex_pose":
                                    break;
                                case "buy_petnis":
                                    break;
                                case "start_npc_sex":
                                    break;
                                case "picture_info":
                                case "picture_info_new":
                                    XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                    XR.Read();
                                    string oo = "";
                                    string d = NastaveniMnfPic.MainFile + XR.GetAttribute("poster_name");
                                    if (!Directory.Exists(d)) Directory.CreateDirectory(d);
                                    if (ActualPicture != "")
                                    {
                                        oo = d + "\\" + ActualPicture.Split('/')[5];
                                        if (!File.Exists(oo))
                                        {
                                            try
                                            {
                                                wc.DownloadFile(new Uri(ActualPicture.Replace(".jpg", "_.jpg")), oo);
                                                SavedPictures++;
                                            }
                                            catch (Exception ex) { Console.WriteLine(ex.Message); Console.WriteLine("nestazeno vse"); }
                                        }
                                    }
                                    ActualPicture = "";
                                    break;
                                case "cant_get_item":
                                    break;
                                case "timeout":
                                    Console.WriteLine("Timeouted");
                                    break;
                                case "disconnect":
                                    Console.WriteLine("Disconnected");
                                    break;
                                case "avatar_data_by_name":
                                    break;
                                case "transfer_money_success":
                                    break;
                                case "start_shower_sex":
                                    break;
                                case "set_default_bg":
                                    break;
                                default:
                                    if (v.Substring(0, 1) != "/") Console.WriteLine("Unknown type: " + v + " in " + s);
                                    break;
                            }
                        }
                    }
                }
                catch (IOException te) { }
                catch (Exception ex) { Console.WriteLine("T:" + ex.Message); }
            }
            e.Cancel = true;
        }

        private void GameBW_DoWork(object sender, DoWorkEventArgs e)
        {
            XmlReader XR = null;
            try
            {
                //lock (MP.Server.LockerChat)
                //    if (!MP.Server.TC_chat.Connected) MP.Server.TC_chat.Connect(MP.Server.AdresaIP, MP.Server.chat_socket);
                //var s = MP.Avatar.icon_id + "," + MP.Uzivatel.LoginPaswCrypted + "," + ActualArea.IdLokace + "," + MP.Server.Session_id + "," + MP.Avatar.userCT;
                //private s+= ",l";
                //ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

                while (!GameBW.CancellationPending)
                {
                    if (!MP.Server.TC_game.Connected) break;
                    try
                    {
                        //lock (MP.Server.LockerGame)
                        //    if (!MP.Server.TC_game.Connected) MP.Server.TC_game.Connect(MP.Server.AdresaIP, MP.Server.game_socket);
                        var ns = MP.Server.TC_game.GetStream();
                        ns.ReadTimeout = 1000;
                        var b = new byte[65535];
                        var i = ns.Read(b, 0, b.Length);
                        var s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
                        if (s != "") Console.WriteLine("G: " + s);
                        var ss = s.Replace("\0", "").Replace("<", "").Split('>');
                        foreach (var v in ss)
                        {
                            if (v != "")
                            {
                                string[] vv = v.Split(' ');
                                switch (vv[0])
                                {
                                    case "game_over":
                                        GamesBW.CancelAsync();
                                        if(GameRepeat)
                                        {
                                            SpinWait.SpinUntil(() => !GamesBW.IsBusy);
                                            Write(MP.Server.TC_game, "<data status=\"start\" />");
                                        }
                                        break;
                                    case "start":
                                        XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                        XR.Read();
                                        if (XR.GetAttribute("success") == "1")
                                            if (!GamesBW.IsBusy) GamesBW.RunWorkerAsync();
                                        break;
                                    default:
                                        Console.WriteLine("G: Unknowen name" + XR.Name);
                                        break;
                                }
                            }
                        }
                    }
                    catch (IOException te) { }
                    catch (Exception ex) { Console.WriteLine("G: " + ex.Message); }
                }
                if(GameBW.CancellationPending)e.Cancel = true;
            }
            catch (Exception ex) { Console.WriteLine("G: " + ex.Message); }
        }

        private void GameBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("GameBW_RunWorkerCompleted");
            if (e.Cancelled)
            {
                GameRepeat = false;
                GamesBW.CancelAsync();
                Write(MP.Server.TC_game, "<data status=\"stop\" />");
            }
        }

        private void GamesBW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (GameID == 2)
                {
                    List<int> body = new List<int>() { 10, 15 };//, 20, 30 };//, 30, 45, 60, 80, 90, 100 };
                    Random r = new Random();
                    //h==75
                    int h = 0;
                    int multiple = 1;
                    while (!GamesBW.CancellationPending)
                    {
                        if (!MP.Server.TC_game.Connected) break;
                        int rn = r.Next(0, 101);
                        if (rn > 60) multiple += multiple >= 4 ? 0 : 1;
                        else if (rn < 40) multiple -= multiple <= 1 ? 0 : 1;
                        var s = "<data scores=\"" + body[r.Next(body.Count)] * multiple + "\" />";
                        Console.WriteLine(h++ + " " + s);
                        if (h == 10) body.Add(20);
                        else if (h == 30) body.Add(30);
                        Write(MP.Server.TC_game, s);
                        Thread.Sleep(r.Next(3000, 4000));
                    }
                    if(GamesBW.CancellationPending)e.Cancel = true;
                }
            }
            catch(Exception ex) { Console.WriteLine("GamesBW error: " + ex.Message); }
        }
        private void GamesBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("GamesBW_RunWorkerCompleted");
            if (e.Cancelled)
            {
                //if (GameRepeat) BeachGameClick(null, null);
            }
        }


        public void GoToArea(MnfLocation area)
        {
            if (ActualArea == area) return;
            ChatBW.CancelAsync();
            AreaBW.CancelAsync();
            Dispatcher.BeginInvoke((Action)(() => { AktualniPostavy.Clear(); }));
            ActualArea = area;
            LastPoint = ActualArea.PortLokace;
            //lock (MP.Server.LockerTop)
            //    if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            var s = "<data area_info=\"" + ActualArea.JmenoLokace + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

            lock (MP.Server.LockerChat)
            {
                if (MP.Server.TC_chat.Connected) MP.Server.TC_chat.Close();
                MP.Server.TC_chat = new TcpClient();
                MP.Server.TC_chat.Connect(MP.Server.AdresaIP, MP.Server.chat_socket);
            }
            s = MP.Avatar.AvatarID + "," + MP.Uzivatel.LoginPaswCrypted + "," + ActualArea.IdLokace + "," + MP.Server.Session_id + "," + MP.Avatar.userCT;
            //private s+= ",l";
            ns = MP.Server.TC_chat.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
            SpinWait.SpinUntil(() => !ChatBW.IsBusy, 1000);
            if (!ChatBW.IsBusy) ChatBW.RunWorkerAsync();

            lock (MP.Server.LockerArea)
            {
                if (MP.Server.TC_area.Connected) MP.Server.TC_area.Close();
                MP.Server.TC_area = new TcpClient();
                MP.Server.TC_area.Connect(MP.Server.AdresaIP, MP.Server.area_socket);
            }
            s = "<data type=\"area\" avatar_id=\"" + MP.Avatar.AvatarID + "\" password=\"" + MP.Uzivatel.LoginPaswCrypted + "\" area_id=\"" + ActualArea.IdLokace + "\" points=\"" + (ActualArea.PortLokace.X + r.Next(0, (int)ActualArea.PortPresnost.X)) + "," + (ActualArea.PortLokace.Y + r.Next(0, (int)ActualArea.PortPresnost.Y)) + "\" bed_ids =\"\" pole_ids=\"\" session_id=\"" + MP.Server.Session_id + "\" />";
            ns = MP.Server.TC_area.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

            SpinWait.SpinUntil(() => !AreaBW.IsBusy, 1000);
            if (!AreaBW.IsBusy) AreaBW.RunWorkerAsync();
        }

        public void WriteA(TcpClient tc, string s)
        {
            try
            {
                NetworkStream ns = tc.GetStream();
                ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
            }
            catch (Exception e) { Console.WriteLine("W: " + e.Message); }
        }
        public void Write(TcpClient tc, string s)
        {
            try
            {
                NetworkStream ns = tc.GetStream();
                var b = Encoding.UTF8.GetBytes(s);
                ns.Write(b, 0, b.Length);
            }
            catch (Exception e) { Console.WriteLine("W: " + e.Message); }
        }
        public void GetAvatarDetails(int AvatarID)
        {
            Write(MP.Server.TC_top, "<data avatar_details=\"1\" id=\"" + AvatarID + "\" />");
        }
        public void GetAvatarFullDetails(int AvatarID)
        {
            Write(MP.Server.TC_top, "<data avatar_details_and_msg_history=\"1\" id=\"" + AvatarID + "\" />");
        }
        public void DeliveryApprove(int FromAvatarID)
        {
            Write(MP.Server.TC_top, "<data private_message=\"delivery_approve\" id_from=\"" + FromAvatarID + "\" />");
        }
        public void UpdateAvatarInfo(string FlagName,string About)
        {
            Write(MP.Server.TC_top, "<data from=\"" + FlagName + "\" about=\"" + About + "\" />");
        }
        public void SendMsg(string msg)
        {
            Write(MP.Server.TC_chat, "m " + msg);
        }
        public void ShowPetnis(bool hide)
        {
            Write(MP.Server.TC_top, "<data hide_petnis=\"" + (hide ? "1" : "0") + "\" />");
        }
        public void setNerusit(bool nerusit)
        {
            Write(MP.Server.TC_top, "<data dont_disturb=\"" + (nerusit ? "1" : "0") + "\" />");
        }

        public void ToJail(int AvatarID)
        {
            ToJail(AvatarID, 60);
        }
        public void ToJail(int AvatarID,int sec)
        {
            Write(MP.Server.TC_top, "<data jail_avatar_id=\"" + AvatarID + "\" time=\"" + sec + "\" />");
        }
        public void FromJail(int AvatarID)
        {
            Write(MP.Server.TC_top, "<data release_avatar_from_jail_id=\"" + AvatarID + "\" />");
        }
        public void SendPMsg(int AvatarID,string msg)
        {
            Write(MP.Server.TC_top, "<data private_message=\"send\" id_to=\"" + AvatarID + "\" text=\"" + msg + "\" />");
        }
        public void MulSendPMsg(int AvatarID, string type)
        {
            Write(MP.Server.TC_top, "<data private_message=\"send\" type=\"" + type + "\" id_to=\"" + AvatarID + "\" />");
        }
        public void RemoveFriend(int AvatarID)
        {
            MulSendPMsg(AvatarID, "friend_remove");
        }
        public void SetIgnor(int AvatarID)
        {
            MulSendPMsg(AvatarID, "set_ignore");
        }
        public void RemIgnor(int AvatarID)
        {
            MulSendPMsg(AvatarID, "stop_ignore");
        }
        public void History(int AvatarID)
        {
            MulSendPMsg(AvatarID, "history");
        }

        public Point LastPoint;
        public void GoToPoint(Point bod)
        {
            Write(MP.Server.TC_area, "<data points=\"" + LastPoint.X + "," + LastPoint.Y + "," + bod.X + "," + bod.Y + "\" />");
            LastPoint = bod;            
        }

        private void comboBoxArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxArea.SelectedIndex < 0) return;
            GoToArea(MnfArea.Lokace[comboBoxArea.SelectedIndex]);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spHistory.Children.Clear();
            if (lbChatPostavy.SelectedIndex < 0)
            { ActivChatAvatar = null; return; }
            else ActivChatAvatar = ChatPostavy[lbChatPostavy.SelectedIndex];
            GetAvatarFullDetails(ActivChatAvatar.AvatarID);
            //if (!Directory.Exists("HistMsg")) Directory.CreateDirectory("HistMsg");
            //var fileName = "HistMsg\\" + ActivChatAvatar.AvatarID + "_" + ActivChatAvatar.JmenoPostavy + ".txt";
            //if (!File.Exists(fileName)) File.Create(fileName).Close();
            //StreamReader sr = new StreamReader(fileName);
            //string radek = sr.ReadLine();
            //while (radek != null)
            //{
            //    var s = radek.Split(' ')[0];
            //    if (s == "snd") spHistory.Children.Add(new Label()
            //    {
            //        Content = radek.Substring(3),
            //        HorizontalAlignment = HorizontalAlignment.Right,
            //        Background = Brushes.LightBlue,
            //    });
            //    else spHistory.Children.Add(new Label()
            //    {
            //        Content = radek.Substring(3),
            //        HorizontalAlignment = HorizontalAlignment.Left,
            //        Background = Brushes.LightPink,
            //    });
            //    radek = sr.ReadLine();
            //}
            //sr.Close();
            //if (MP != null)
            //    DeliveryApprove(MP.Avatar.AvatarID);
        }
        
        private void SendClick(object sender, RoutedEventArgs e)
        {
            if (tbChat.Text == null || tbChat.Text == "" || ActivChatAvatar == null) return;
            SendPMsg(ActivChatAvatar.AvatarID, tbChat.Text);
            LogMsg(ActivChatAvatar, tbChat.Text, false);
            spHistory.Children.Add(new Label()
            {
                Content = tbChat.Text,
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = Brushes.LightBlue,
            });
            tbChat.Text = "";
        }
        public void LogMsg(MnfAvatar avatarID,string msg, bool recv)
        {
            if (!Directory.Exists("HistMsg")) Directory.CreateDirectory("HistMsg");
            StreamWriter sw = new StreamWriter("HistMsg\\" + avatarID.AvatarID + "_" + avatarID.JmenoPostavy + ".txt", true);
            if (recv) sw.WriteLine("rcv " + msg);
            else sw.WriteLine("snd " + msg);
            sw.Close();
        }

        private void AvatarInAreaSelected(object sender, SelectionChangedEventArgs e)
        {
            if (lbAktualniPostavy.SelectedIndex < 0) return;
            MnfAvatar ma = AktualniPostavy[lbAktualniPostavy.SelectedIndex];
            var v = (from f in PotkanePostavy where f.AvatarID == ma.AvatarID select f).ToArray();
            if (v.Count() > 0)
            {
                InfoAvatar = v[0];
                GetAvatarFullDetails(v[0].AvatarID);
            }
        }

        private void ListBoxAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbPotkanePostavy.SelectedIndex < 0) return;
            MnfAvatar ma = PotkanePostavy[lbPotkanePostavy.SelectedIndex];
            InfoAvatar = ma;
            GetAvatarFullDetails(ma.AvatarID);
        }

        private void addToChat(object sender, RoutedEventArgs e)
        {
            if (lbPotkanePostavy.SelectedIndex < 0) return;
            MnfAvatar ma = PotkanePostavy[lbPotkanePostavy.SelectedIndex];
            ChatPostavy.Add(ma);
        }

        private void getList(object sender, RoutedEventArgs e)
        {
            Write(MP.Server.TC_top, "<data friends_list=\"1\" />");
        }

        private void BeachGameClick(object sender, RoutedEventArgs e)
        {
            if (GameBW.IsBusy) return;
            if (GameID == 2)
            {
                GoToArea(MnfArea.Lokace[3]);
                ChatBW.CancelAsync();
                AreaBW.CancelAsync();
                if (MP.Server.TC_area.Connected) MP.Server.TC_area.Close();
                if (MP.Server.TC_chat.Connected) MP.Server.TC_chat.Close();
            }
            if (MP.Server.TC_game.Connected) MP.Server.TC_game.Close();
            MP.Server.TC_game = new TcpClient();
            MP.Server.TC_game.Connect(MP.Server.AdresaIP, MP.Server.game_socket);

            Write(MP.Server.TC_game, "<data avatar_id=\"" + MP.Avatar.AvatarID + "\" password=\"" + MP.Uzivatel.LoginPaswCrypted + "\" game_id=\"" + GameID + "\" session_id=\"" + MP.Server.Session_id + "\" />");
            Write(MP.Server.TC_top, "<data cash_display = '1' />");
            Thread.Sleep(2000);
            Write(MP.Server.TC_game, "<data status=\"start\" />");

            if (!GameBW.IsBusy) GameBW.RunWorkerAsync();
        }

        private void StopGameClick(object sender, RoutedEventArgs e)
        {
            GameBW?.CancelAsync();
        }
        private void PictureBW_DoWork(object sender, DoWorkEventArgs e)
        {
            SavedPictures = 0;
            GoToArea(MnfArea.Lokace[19]);
            if (PictureBW.CancellationPending) return;
            GoToArea(MnfArea.Lokace[20]);
            if (PictureBW.CancellationPending) return;
            SavePicture = true;
            GoToArea(MnfArea.Lokace[21]);
            if (PictureBW.CancellationPending) return;
            SpinWait.SpinUntil(() => SavePicture == false || PictureBW.CancellationPending, 20000);
            SavePicture = true;
            GoToArea(MnfArea.Lokace[22]);
            if (PictureBW.CancellationPending) return;
            SpinWait.SpinUntil(() => SavePicture == false || PictureBW.CancellationPending, 20000);
            SavePicture = true;
            GoToArea(MnfArea.Lokace[23]);
            if (PictureBW.CancellationPending) return;
            SpinWait.SpinUntil(() => SavePicture == false || PictureBW.CancellationPending, 20000);
            SavePicture = true;
            GoToArea(MnfArea.Lokace[24]);
            if (PictureBW.CancellationPending) return;
            SpinWait.SpinUntil(() => SavePicture == false || PictureBW.CancellationPending, 20000);
        }

        public void GetAllPicture()
        {
            getPicture(null, null);
        }

        private void getPicture(object sender, RoutedEventArgs e)
        {
            if(!PictureBW.IsBusy) PictureBW.RunWorkerAsync();
        }

        private void ChatRemoveClick(object sender, RoutedEventArgs e)
        {
            ChatPostavy.Remove(ActivChatAvatar);
        }
        //"<avatar data=\"1168373,SissySlutEric,2,2,2,8,3,38,38,1,3,2,1,1,6,3,1,5,2,3,1,3,1,7,176/252/200,1217,1217,-1,,0,0,7,38,2,2,40,38,0,1\" points=\"987,1179,987,1181\" /><avatar data=\"2278205,Teranas,1,3,2,8,1,38,1,1,1,3,2,2,6,2,1,1,3,5,3,1,3,9,176/252/208,8120,620,-1,,0,1,1,0,0,3,38,0,0,1\" points=\"1130,721,1183,723\" /><avatar data=\"3898264,wolfkidd,1,3,1,7,2,35,9,2,2,1,2,2,6,3,1,5,3,1,1,1,1,3,239/176/252,5609,4859,-1,,0,0,11,35,9,3,38,1,0,1\" points=\"764,735,884,676\" /><avatar data=\"2515041,Casslut,2,2,5,5,0,37,37,1,1,3,1,2,7,1,1,4,3,2,1,3,3,7,202/176/252,2771,2021,-1,,0,0,1,0,0,1,0,0,0,1\" points=\"928,691\" />\0"
        //"<avatar_out id=\"2515041\" />\0"
        //"<avatar data=\"3969460,AmandaLove,2,1,1,2,1,24,37,3,3,2,1,2,2,3,1,5,1,4,2,3,4,2,228/176/252,1789,1039,-1,,0,0,10,37,32,1,0,0,0,1\" points=\"1071,689\" />\0<avatar_out id=\"3969460\" />\0<avatar_out id=\"3898264\" />\0<avatar data=\"99742,VzoreCZEk,1,2,2,8,1,38,39,2,2,1,1,2,6,3,1,4,3,1,3,1,1,10,177/176/252,11649,4149,38,GirlsTrueLove,0,1,13,38,39,1,0,0,0,1\" points=\"1092,726\" />\0<avatar id=\"99742\" points=\"1092,726,1354.3,701.65,1354.3,701.65,1337,506\" />\0<avatar id=\"99742\" points=\"1338.64,524.6,1315.3,469.1,1315.3,469.1,1186,333\" />\0<avatar id=\"99742\" points=\"1260.32,411.23,949,247\" />\0<avatar_out id=\"99742\" />\0"
        //"<avatar id=\"1168373\" points=\"987,1181,986,1139\" />\0<avatar id=\"1168373\" points=\"986,1139,989,1121\" />\0<avatar data=\"3946816,Scumbag,1,1,9,2,4,40,1,1,2,1,1,1,5,3,1,3,1,2,3,1,1,8,252/176/194,4569,3819,-1,,0,0,1,0,0,1,0,0,0,1\" points=\"1664,680\" />\0<avatar data=\"2869957,Zephora,2,2,6,5,2,38,40,1,2,2,1,2,3,3,1,4,2,3,1,3,4,3,252/176/178,8269,7519,-1,,0,0,7,38,40,2,36,33,0,1\" points=\"1161,712\" />\0<avatar id=\"3946816\" points=\"1664,680,1197,802\" />\0<avatar id=\"3946816\" points=\"1494.75,724.22,1197,843\" />\0<avatar id=\"2869957\" points=\"1161,712,1064,692\" />\0<avatar id=\"2869957\" points=\"1064,692,972,680\" />\0<avatar id=\"2869957\" points=\"972,680,889,670\" />\0<avatar id=\"3946816\" points=\"1197,843,1208,1017\" />\0<avatar id=\"2869957\" points=\"889,670,815,696\" />\0<avatar id=\"2869957\" points=\"815,696,746,735\" />\0<avatar id=\"3946816\" points=\"1208,1017,1209,1190\" />\0"
    }
}
