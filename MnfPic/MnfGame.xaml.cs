using System;
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
            //GameBW = new BackgroundWorker();
            GameBW.WorkerSupportsCancellation = true;
            GameBW.DoWork += GameBW_DoWork;
            GameBW.RunWorkerCompleted += GameBW_RunWorkerCompleted;

            PictureBW.WorkerSupportsCancellation = true;
            PictureBW.DoWork += PictureBW_DoWork;
            //PictureBW.RunWorkerCompleted += GameBW_RunWorkerCompleted;
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
        bool Disconect = false;
        bool Disconect2 = false;
        bool Disconect3 = false;

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
        public BackgroundWorker GameBW { get { return _GameBW; } private set { _GameBW = value; } }
        BackgroundWorker _PictureBW = new BackgroundWorker();
        public BackgroundWorker PictureBW { get { return _PictureBW; } private set { _PictureBW = value; } }
        public void Init(MnfPlayer mp)
        {
            Disconect = false;
            MP = mp;
            string s;

            try
            {
                //get policy
                //mp.Server.TC_policy = new TcpClient();
                lock (MP.Server.LockerPolicy)
                mp.Server.TC_policy.Connect(mp.Server.AdresaIP, mp.Server.policy_socket);
                NetworkStream ns = mp.Server.TC_policy.GetStream();
                byte[] b = new byte[65535];
                int i = ns.Read(b, 0, b.Length);
                s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
                //Console.WriteLine("Policy: " + s);
                mp.Server.TC_policy.Close();
            
                try
                {

                    new Thread(DataReaderTop).Start();
                    //new Thread(DataReaderArea).Start();
                    //new Thread(DataReaderChat).Start();

                    //ns.ReadTimeout = 1000;
                    //b = new byte[65535];
                    //i = ns.Read(b, 0, b.Length);
                    //s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);

                    //ss = s.Split('\"');
                    //DateTime.TryParse(ss[1], out MP.Server.Server_time);//overit funkcnost?
                    //MP.Server.Session_id = ss[3];

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    }
                }
            catch (Exception e)
            {
                Console.WriteLine("Polisy error");
                MessageBox.Show(e.ToString());
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

            Disconect = true;
            Disconect2 = true;
            if (MP == null) return;
            lock(MP.Server.LockerChat)  if (MP.Server.TC_chat.Connected) MP.Server.TC_chat.Close();
            lock (MP.Server.LockerArea) if (MP.Server.TC_area.Connected) MP.Server.TC_area.Close();
            lock (MP.Server.LockerGame) if (MP.Server.TC_game.Connected) MP.Server.TC_game.Close();
            lock (MP.Server.LockerTop) if (MP.Server.TC_top.Connected) MP.Server.TC_top.Close();
            lock (MP.Server.LockerPolicy) if (MP.Server.TC_policy.Connected) MP.Server.TC_policy.Close();
            sw.Stop();
            Console.WriteLine("Elapsed time " + sw.Elapsed);
            MP.Server.TC_chat = new TcpClient();
            MP.Server.TC_area = new TcpClient();
            MP.Server.TC_game = new TcpClient();
            MP.Server.TC_top = new TcpClient();
            MP.Server.TC_policy = new TcpClient();
        }

        public DateTime GetActualServerTime()
        {
            return MP.Server.Server_time.AddMilliseconds(sw.ElapsedMilliseconds);
        }

        private void DataReaderTop()
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
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment,
                    //CheckCharacters = false,
                };
                settings.ValidationEventHandler += Settings_ValidationEventHandler;

                while (!Disconect)
                {
                    try
                    {
                        lock (MP.Server.LockerTop)
                            if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
                        s = "<data avatar_id=\"" + MP.Avatar.AvatarID + "\" password=\"" + MP.Uzivatel.LoginPaswCrypted + "\" />";
                        NetworkStream ns = MP.Server.TC_top.GetStream();
                        ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                        ns.ReadTimeout = 500;
                        //XmlReader XR = XmlReader.Create(ns, settings);
                        ns = MP.Server.TC_top.GetStream();
                        byte[] b = new byte[16777216];
                        int i = ns.Read(b, 0, b.Length);
                        s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
                        if (s != "") Console.WriteLine("T: " + s);
                        ss = s.Replace("\0", "").Replace("<", "").Split('>');
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
                                                catch (Exception e) { Console.WriteLine(e); Console.WriteLine("nestazeno vse"); }
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
                                        Console.WriteLine("Unknown type: " + s);
                                        break;
                                }
                            }
                        }
                    }
                    catch (IOException te) { }
                    catch (Exception ex) { Console.WriteLine("T:" + ex.Message); }
                }

            }
            catch (Exception ex) { Console.WriteLine("T:" + ex.Message); }

        }

        private void Settings_ValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DataReaderArea()
        {
            Disconect2 = false;
            try
            {
                //lock (MP.Server.LockerArea)
                //    if (!MP.Server.TC_area.Connected) MP.Server.TC_area.Connect(MP.Server.AdresaIP, MP.Server.area_socket);
                //Random r = new Random();
                //var s = "<data type=\"area\" avatar_id=\"" + MP.Avatar.AvatarID + "\" password=\"" + MP.Uzivatel.LoginPaswCrypted + "\" area_id=\"" + ActualArea.IdLokace + "\" points=\"" + (ActualArea.PortLokace.X + r.Next(-5, 5)) + "," + (ActualArea.PortLokace.Y + r.Next(-5, 5)) + "\" bed_ids =\"\" pole_ids=\"\" session_id=\"" + MP.Server.Session_id + "\" />";
                
                //ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment,
                    CheckCharacters = false,
                };
                XmlReader XR = null;
                while (!Disconect2)
                {
                    try
                    {
                        lock (MP.Server.LockerArea)
                            if (!MP.Server.TC_area.Connected) MP.Server.TC_area.Connect(MP.Server.AdresaIP, MP.Server.area_socket);
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
                                        if (data!=null)
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
                                                lock (MP.Server.LockerTop)
                                                    if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
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
            }
            catch (Exception ex) { Console.WriteLine("A: " + ex.Message); }
        }
        private void DataReaderChat()
        {
            Disconect2 = false;
            try
            {
                //lock (MP.Server.LockerChat)
                //    if (!MP.Server.TC_chat.Connected) MP.Server.TC_chat.Connect(MP.Server.AdresaIP, MP.Server.chat_socket);
                //var s = MP.Avatar.icon_id + "," + MP.Uzivatel.LoginPaswCrypted + "," + ActualArea.IdLokace + "," + MP.Server.Session_id + "," + MP.Avatar.userCT;
                //private s+= ",l";
                var ns = MP.Server.TC_chat.GetStream();
                //ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                ns.ReadTimeout = 500;
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment,
                    CheckCharacters = false,
                };
                XmlReader XR = XmlReader.Create(ns, settings);
                while (!Disconect2)
                {
                    try
                    {
                        while (XR.Read())
                        {
                            if (XR.NodeType == XmlNodeType.Element)
                            {
                                switch (XR.Name)
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
                                        Console.WriteLine("C: Unknowen name" + XR.Name);
                                        break;
                                }
                            }
                        }
                    }
                    catch (IOException te) { }
                    catch (Exception ex) { Console.WriteLine("C: " + ex.Message); }
                }
            }
            catch (Exception ex) { Console.WriteLine("C: " + ex.Message); }
        }
        private void DataReaderGame()
        {
            Disconect3 = false;
            try
            {
                //lock (MP.Server.LockerChat)
                //    if (!MP.Server.TC_chat.Connected) MP.Server.TC_chat.Connect(MP.Server.AdresaIP, MP.Server.chat_socket);
                //var s = MP.Avatar.icon_id + "," + MP.Uzivatel.LoginPaswCrypted + "," + ActualArea.IdLokace + "," + MP.Server.Session_id + "," + MP.Avatar.userCT;
                //private s+= ",l";
                //ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment,
                    CheckCharacters = false,
                };
                XmlReader XR = null;
                while (!Disconect3)
                {
                    try
                    {
                        lock (MP.Server.LockerGame)
                            if (!MP.Server.TC_game.Connected) MP.Server.TC_game.Connect(MP.Server.AdresaIP, MP.Server.game_socket);
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
                                        GameBW.CancelAsync();
                                        break;
                                    case "start":
                                        XR = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes("<" + v + ">")), settings);
                                        XR.Read();
                                        if (XR.GetAttribute("success") == "1")
                                            if (!GameBW.IsBusy) GameBW.RunWorkerAsync();
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
            }
            catch (Exception ex) { Console.WriteLine("G: " + ex.Message); }
        }

        private void GameBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (GameID == 2)
            {
                List<int> body = new List<int>() { 10, 15 };//, 20, 30 };//, 30, 45, 60, 80, 90, 100 };
                Random r = new Random();
                //h==75
                int h = 0;
                int multiple = 1;
                while (!GameBW.CancellationPending)
                {
                    int rn = r.Next(0, 101);
                    if (rn > 60) multiple += multiple >= 4 ? 0 : 1;
                    else if (rn < 40) multiple -= multiple <= 1 ? 0 : 1;
                    var s = "<data scores=\"" + body[r.Next(body.Count)] * multiple + "\" />";
                    Console.WriteLine(h++ + " " + s);
                    if (h == 10) body.Add(20);
                    else if (h == 30) body.Add(30);
                    lock (MP.Server.LockerGame)
                        if (!MP.Server.TC_game.Connected) MP.Server.TC_game.Connect(MP.Server.AdresaIP, MP.Server.game_socket);
                    var ns = MP.Server.TC_game.GetStream();
                    ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                    Thread.Sleep(r.Next(3000, 4000));
                }
            }
        }

        private void GameBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MP.Server.TC_game.Close();
            Disconect3 = true;
            if (GameRepeat) BeachGameClick(null, null);
        }

        public void GoToArea(MnfLocation area)
        {
            if (ActualArea == area) return;
            Dispatcher.BeginInvoke((Action)(() => { AktualniPostavy.Clear(); }));
            Disconect2 = true;
            string s;
            ActualArea = area;
            LastPoint = ActualArea.PortLokace;
            //lock (MP.Server.LockerTop)
            //    if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            s = "<data area_info=\"" + ActualArea.JmenoLokace + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

            lock (MP.Server.LockerChat)
            {
                if (MP.Server.TC_chat.Connected) MP.Server.TC_chat.Close();
                MP.Server.TC_chat = new TcpClient();
                if (!MP.Server.TC_chat.Connected) MP.Server.TC_chat.Connect(MP.Server.AdresaIP, MP.Server.chat_socket);
            }
            s = MP.Avatar.AvatarID + "," + MP.Uzivatel.LoginPaswCrypted + "," + ActualArea.IdLokace + "," + MP.Server.Session_id + "," + MP.Avatar.userCT;
            //private s+= ",l";
            ns = MP.Server.TC_chat.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
            new Thread(DataReaderChat).Start();

            lock (MP.Server.LockerArea)
            {
                if (MP.Server.TC_area.Connected) MP.Server.TC_area.Close();
                MP.Server.TC_area = new TcpClient();
                if (!MP.Server.TC_area.Connected) MP.Server.TC_area.Connect(MP.Server.AdresaIP, MP.Server.area_socket);
            }
            s = "<data type=\"area\" avatar_id=\"" + MP.Avatar.AvatarID + "\" password=\"" + MP.Uzivatel.LoginPaswCrypted + "\" area_id=\"" + ActualArea.IdLokace + "\" points=\"" + (ActualArea.PortLokace.X + r.Next(-5, 5)) + "," + (ActualArea.PortLokace.Y + r.Next(-5, 5)) + "\" bed_ids =\"\" pole_ids=\"\" session_id=\"" + MP.Server.Session_id + "\" />";
            ns = MP.Server.TC_area.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
            new Thread(DataReaderArea).Start();
        }
        public void GetAvatarDetails(int AvatarID)
        {
            //_root.top_socket.send("<data avatar_details=\"1\" id=\"" + avatar_id + "\" />");
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data avatar_details=\"1\" id=\"" + AvatarID + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void GetAvatarFullDetails(int AvatarID)
        {
            // _root.top_socket.send("<data avatar_details_and_msg_history=\"1\" id=\"" + avatar_id + "\" />");
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data avatar_details_and_msg_history=\"1\" id=\"" + AvatarID + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void DeliveryApprove(int FromAvatarID)
        {
            //_root.top_socket.send("<data private_message=\"delivery_approve\" id_from=\"" + _loc4_.id + "\" />");
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data private_message=\"delivery_approve\" id_from=\"" + FromAvatarID + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void UpdateAvatarInfo(string FlagName,string About)
        {
            // _root.top_socket.send("<data from=\"" + flag.flag_name + "\" about=\"" + _loc2_.text + "\" />");
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data from=\"" + FlagName + "\" about=\"" + About + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void SendMsg(string msg)
        {
            lock (MP.Server.LockerChat)
                if (!MP.Server.TC_chat.Connected) MP.Server.TC_chat.Connect(MP.Server.AdresaIP, MP.Server.chat_socket);
            string s = "m" + msg;
            NetworkStream ns = MP.Server.TC_chat.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void ShowPetnis(bool hide)
        {
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data hide_petnis=\"" + (hide ? "1" : "0") + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void setNerusit(bool nerusit)
        {
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data dont_disturb=\"" + (nerusit ? "1" : "0") + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }

        public void ToJail(int AvatarID)
        {
            ToJail(AvatarID, 60);
        }
        public void ToJail(int AvatarID,int sec)
        {
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data jail_avatar_id=\"" + AvatarID + "\" time=\"" + sec + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void FromJail(int AvatarID)
        {
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data release_avatar_from_jail_id=\"" + AvatarID + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void SendPMsg(int AvatarID,string msg)
        {
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data private_message=\"send\" id_to=\"" + AvatarID + "\" text=\"" + msg + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void RemoveFriend(int AvatarID)
        {
            //_root.top_socket.send("<data private_message=\"delivery_approve\" id_from=\"" + _loc4_.id + "\" />");
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data private_message=\"send\" type=\"friend_remove\" id_to=\"" + AvatarID + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void SetIgnor(int AvatarID)
        {
            //_root.top_socket.send("<data private_message=\"delivery_approve\" id_from=\"" + _loc4_.id + "\" />");
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data private_message=\"send\" type=\"set_ignore\" id_to=\"" + AvatarID + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void RemIgnor(int AvatarID)
        {
            //_root.top_socket.send("<data private_message=\"delivery_approve\" id_from=\"" + _loc4_.id + "\" />");
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data private_message=\"send\" type=\"stop_ignore\" id_to=\"" + AvatarID + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }
        public void History(int AvatarID)
        {
            //_root.top_socket.send("<data private_message=\"delivery_approve\" id_from=\"" + _loc4_.id + "\" />");
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            string s = "<data private_message=\"send\" type=\"history\" id_to=\"" + AvatarID + "\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }

        public Point LastPoint;
        public void GoToPoint(Point bod)
        {
            string s;
            lock (MP.Server.LockerArea)
                if (!MP.Server.TC_area.Connected) MP.Server.TC_area.Connect(MP.Server.AdresaIP, MP.Server.area_socket);
            //<data points="700.96,605.23,948,631" />
            s = "<data points=\"" + LastPoint.X + "," + LastPoint.Y + "," + bod.X + "," + bod.Y + "\" />";
            LastPoint = bod;
            NetworkStream ns = MP.Server.TC_area.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
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
            string s;
            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            //<data points="700.96,605.23,948,631" />
            s = "<data friends_list=\"1\" />";
            NetworkStream ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        }

        private void BeachGameClick(object sender, RoutedEventArgs e)
        {
            if (GameID == 2) GoToArea(MnfArea.Lokace[3]);
            string s;
            if (MP.Server.TC_game.Connected) MP.Server.TC_game.Close();
            MP.Server.TC_game = new TcpClient();

            lock (MP.Server.LockerGame)
                if (!MP.Server.TC_game.Connected) MP.Server.TC_game.Connect(MP.Server.AdresaIP, MP.Server.game_socket);
            s = "<data avatar_id=\"" + MP.Avatar.AvatarID + "\" password=\"" + MP.Uzivatel.LoginPaswCrypted + "\" game_id=\"" + GameID + "\" session_id=\"" + MP.Server.Session_id + "\" />";
            var ns = MP.Server.TC_game.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);


            lock (MP.Server.LockerTop)
                if (!MP.Server.TC_top.Connected) MP.Server.TC_top.Connect(MP.Server.AdresaIP, MP.Server.top_socket);
            s = "<data cash_display = '1' />";
            ns = MP.Server.TC_top.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);


            lock (MP.Server.LockerGame)
                if (!MP.Server.TC_game.Connected) MP.Server.TC_game.Connect(MP.Server.AdresaIP, MP.Server.game_socket);
            s = "<data status=\"start\" />";
            ns = MP.Server.TC_game.GetStream();
            ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

            new Thread(DataReaderGame).Start();
        }

        private void StopGameClick(object sender, RoutedEventArgs e)
        {
            GameBW?.CancelAsync();
        }
        private void PictureBW_DoWork(object sender, DoWorkEventArgs e)
        {
            SavedPictures = 0;
            GoToArea(MnfArea.Lokace[19]);
            GoToArea(MnfArea.Lokace[20]);
            SavePicture = true;
            GoToArea(MnfArea.Lokace[21]);
            SpinWait.SpinUntil(() => SavePicture == false, 20000);
            SavePicture = true;
            GoToArea(MnfArea.Lokace[22]);
            SpinWait.SpinUntil(() => SavePicture == false, 20000);
            SavePicture = true;
            GoToArea(MnfArea.Lokace[23]);
            SpinWait.SpinUntil(() => SavePicture == false, 20000);
            SavePicture = true;
            GoToArea(MnfArea.Lokace[24]);
            SpinWait.SpinUntil(() => SavePicture == false, 20000);
        }

        public void GetAllPicture()
        {
            getPicture(null, null);
        }
        private void getPicture(object sender, RoutedEventArgs e)
        {
            if(!PictureBW.IsBusy) PictureBW.RunWorkerAsync();
        }

        //public bool PoSlozkach = true;
        //public bool GoToArea(int area)
        //{
        //    string s;

        //    if (TC_chat.Connected) TC_chat.Close();
        //    if (TC_area.Connected) TC_area.Close();

        //    TC_chat = new TcpClient();
        //    TC_area = new TcpClient();

        //    if (!TC_top.Connected) TC_top.Connect(AdresaIP, top_socket);
        //    if (!TC_area.Connected) TC_area.Connect(AdresaIP, area_socket);
        //    //if (!TC_game.Connected) TC_game.Connect(AdresaIP, game_socket);
        //    if (!TC_chat.Connected) TC_chat.Connect(AdresaIP, chat_socket);

        //    s = "<data area_info=\"" + mnfArea.Jmena[area] + "\" />";
        //    NetworkStream ns = TC_top.GetStream();
        //    ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

        //    s = "";
        //    s += Avatar.AvatarID + ",";
        //    s += Avatar.User.LoginPaswCrypted + ",";
        //    s += mnfArea.GetAreaID(mnfArea.Jmena[area]) + ",";
        //    s += session_id + ",";
        //    s += Avatar.userCT;
        //    ns = TC_chat.GetStream();
        //    ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

        //    s = "<data type=\"area\" avatar_id=\"" + Avatar.AvatarID + "\" password=\"" + Avatar.User.LoginPaswCrypted + "\" area_id=\"" + mnfArea.GetAreaID(mnfArea.Jmena[area]) + "\" points=\"" + (mnfArea.Porty[area].X + r.Next(-5, 5)) + "," + (mnfArea.Porty[area].Y + r.Next(-5, 5)) + "\" bed_ids =\"\" pole_ids=\"\" session_id=\"" + session_id + "\" />";
        //    LastPoint = mnfArea.Porty[area];
        //    //socket.send(((((((((((((((("<data type=\"area\" avatar_id=\"" + my_avatar.data.id) + "\" password=\"") + _root.user_pass) + "\" area_id=\"") + _root.area_id) + "\" points=\"") + my_avatar._x) + ",") + my_avatar._y) + "\" bed_ids =\"") + _local3) + "\" pole_ids=\"") + _local4) + "\" session_id=\"") + _root.session_id) + "\" />");
        //    ns = TC_area.GetStream();
        //    ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        //    ns.ReadTimeout = 500;
        //    byte[] b = new byte[65535];
        //    int i = 0;
        //    do
        //    {
        //        try
        //        {
        //            i = ns.Read(b, 0, b.Length);
        //        }
        //        catch { return false; }
        //        if (i > 0)//smycka pro cteni ... data o lidech a pak fotky ...
        //        {
        //            s = Encoding.UTF8.GetString(Array.ConvertAll(b, x => (byte)x), 0, i);
        //            if (s.Contains("pictures"))
        //            {
        //                int stazeno = 0;
        //                string[] ss = s.Split('\'');
        //                List<string> obrazky = new List<string>();
        //                for (int a = 0; a < ss.Length; a++)
        //                {
        //                    if (a % 2 == 1) obrazky.Add(ss[a]);
        //                }
        //                foreach (string o in obrazky)
        //                {
        //                    if (NastaveniMnfPic.MainFile == "")
        //                    {
        //                        System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
        //                        if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //                        {
        //                            NastaveniMnfPic.MainFile = fbd.SelectedPath + "\\";
        //                        }
        //                    }
        //                    string oo = NastaveniMnfPic.MainFile + o.Split('/')[5];
        //                    //<data get_picture_info='bb1828fc60e44e63ea6889a392b23097'/>//port2030
        //                    //<picture_info poster_name='PaulinaLira' rating='4.3' is_voted='0' />//resp
        //                    if (PoSlozkach)
        //                    {
        //                        s = "<data get_picture_info='" + Path.GetFileNameWithoutExtension(oo) + "'/>";
        //                        ns = TC_top.GetStream();
        //                        ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);

        //                        ns.ReadTimeout = 1000;
        //                        byte[] bb = new byte[65535*8];
        //                        int ii = 0;
        //                        try
        //                        {
        //                            ii = ns.Read(bb, 0, bb.Length);
        //                        }
        //                        catch { }
        //                        if (ii != 0)
        //                        {
        //                            s = Encoding.UTF8.GetString(Array.ConvertAll(bb, x => (byte)x), 0, ii);
        //                            if (s.Contains("poster_name"))
        //                            {
        //                                string[] sss = s.Split('\'');
        //                                string d = NastaveniMnfPic.MainFile + sss[1];
        //                                if (!Directory.Exists(d)) Directory.CreateDirectory(d);
        //                                oo = d + "\\" + o.Split('/')[5];
        //                            }
        //                        }
        //                    }
        //                    if (!File.Exists(oo))
        //                    {
        //                        try
        //                        {
        //                            wc.DownloadFile(new Uri(o.Replace(".jpg", "_.jpg")), oo);
        //                            stazeno++;
        //                        }
        //                        catch (Exception e) { Console.WriteLine(e); Console.WriteLine("nestazeno vse"); }
        //                    }
        //                }
        //                Console.WriteLine(stazeno);
        //            }
        //            //MessageBox.Show(s);
        //        }
        //        if(s.Contains("avatar data"))
        //        {
        //            //
        //            // <avatar data="8648807,AureliaMoon2,2,3,3,48,2,10,24,3,3,2,1,2,5,2,1,1,2,3,2,1,2,9,252/219/176,10637,137,31,Lolly,0,1,1,0,0,1,0,0,1,1" points="816,673" />
        //            string[] ss = s.Split('<');
        //            foreach(var v in ss)
        //            {
        //                if (v.Contains("avatar data"))
        //                {
        //                    var postava = new MnfAvatar(v);
        //                    int IDpostavy = -1;
        //                    for (int p = 0; p < PotkanePostavy.Count; p++)
        //                    {
        //                        if (PotkanePostavy[p].JmenoPostavy == postava.JmenoPostavy)
        //                        {
        //                            IDpostavy = p;
        //                            break;
        //                        }
        //                    }
        //                    if (IDpostavy >= 0)
        //                    {
        //                        //aktualizuj pole
        //                        PotkanePostavy[IDpostavy] = postava;
        //                    }
        //                    else PotkanePostavy.Add(postava);
        //                }
        //            }

        //        }
        //        if (s.Contains("avatar id"))
        //        {
        //            //<avatar id="1451409" points="1341,953,1196.1,1137.25,1196.1,1137.25,1047,1147" />
        //            string[] ss = s.Split(' ');
        //            int postavaID = int.Parse(ss[1].Replace("\"", "").Split('=')[1]);
        //            var postava = (from f in PotkanePostavy where f.AvatarID==postavaID select f).ToList();
        //            if(postava.Count()>0)
        //            {
        //                //Console.WriteLine(postava[0].Name + " se pohla");
        //            }
        //        }
        //    } while (i != 0);
        //    return true;
        //}
        //public Point LastPoint;
        //public void GoToPoint(Point bod)
        //{
        //    string s;
        //    if (!TC_area.Connected) TC_area.Connect(AdresaIP, area_socket);
        //    //<data points="700.96,605.23,948,631" />
        //    s = "<data points=\"" + LastPoint.X + "," + LastPoint.Y + "," + bod.X + "," + bod.Y + "\" />";
        //    LastPoint = bod;
        //    NetworkStream ns = TC_top.GetStream();
        //    ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
        //}
        //"<avatar data=\"1168373,SissySlutEric,2,2,2,8,3,38,38,1,3,2,1,1,6,3,1,5,2,3,1,3,1,7,176/252/200,1217,1217,-1,,0,0,7,38,2,2,40,38,0,1\" points=\"987,1179,987,1181\" /><avatar data=\"2278205,Teranas,1,3,2,8,1,38,1,1,1,3,2,2,6,2,1,1,3,5,3,1,3,9,176/252/208,8120,620,-1,,0,1,1,0,0,3,38,0,0,1\" points=\"1130,721,1183,723\" /><avatar data=\"3898264,wolfkidd,1,3,1,7,2,35,9,2,2,1,2,2,6,3,1,5,3,1,1,1,1,3,239/176/252,5609,4859,-1,,0,0,11,35,9,3,38,1,0,1\" points=\"764,735,884,676\" /><avatar data=\"2515041,Casslut,2,2,5,5,0,37,37,1,1,3,1,2,7,1,1,4,3,2,1,3,3,7,202/176/252,2771,2021,-1,,0,0,1,0,0,1,0,0,0,1\" points=\"928,691\" />\0"
        //"<avatar_out id=\"2515041\" />\0"
        //"<avatar data=\"3969460,AmandaLove,2,1,1,2,1,24,37,3,3,2,1,2,2,3,1,5,1,4,2,3,4,2,228/176/252,1789,1039,-1,,0,0,10,37,32,1,0,0,0,1\" points=\"1071,689\" />\0<avatar_out id=\"3969460\" />\0<avatar_out id=\"3898264\" />\0<avatar data=\"99742,VzoreCZEk,1,2,2,8,1,38,39,2,2,1,1,2,6,3,1,4,3,1,3,1,1,10,177/176/252,11649,4149,38,GirlsTrueLove,0,1,13,38,39,1,0,0,0,1\" points=\"1092,726\" />\0<avatar id=\"99742\" points=\"1092,726,1354.3,701.65,1354.3,701.65,1337,506\" />\0<avatar id=\"99742\" points=\"1338.64,524.6,1315.3,469.1,1315.3,469.1,1186,333\" />\0<avatar id=\"99742\" points=\"1260.32,411.23,949,247\" />\0<avatar_out id=\"99742\" />\0"
        //"<avatar id=\"1168373\" points=\"987,1181,986,1139\" />\0<avatar id=\"1168373\" points=\"986,1139,989,1121\" />\0<avatar data=\"3946816,Scumbag,1,1,9,2,4,40,1,1,2,1,1,1,5,3,1,3,1,2,3,1,1,8,252/176/194,4569,3819,-1,,0,0,1,0,0,1,0,0,0,1\" points=\"1664,680\" />\0<avatar data=\"2869957,Zephora,2,2,6,5,2,38,40,1,2,2,1,2,3,3,1,4,2,3,1,3,4,3,252/176/178,8269,7519,-1,,0,0,7,38,40,2,36,33,0,1\" points=\"1161,712\" />\0<avatar id=\"3946816\" points=\"1664,680,1197,802\" />\0<avatar id=\"3946816\" points=\"1494.75,724.22,1197,843\" />\0<avatar id=\"2869957\" points=\"1161,712,1064,692\" />\0<avatar id=\"2869957\" points=\"1064,692,972,680\" />\0<avatar id=\"2869957\" points=\"972,680,889,670\" />\0<avatar id=\"3946816\" points=\"1197,843,1208,1017\" />\0<avatar id=\"2869957\" points=\"889,670,815,696\" />\0<avatar id=\"2869957\" points=\"815,696,746,735\" />\0<avatar id=\"3946816\" points=\"1208,1017,1209,1190\" />\0"
    }
}
