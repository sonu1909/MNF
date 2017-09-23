using App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MnfPic
{

    public class Uzivatel : ANotify
    {
        //string _Jmeno = "";
        //public string JmenoUzivatele
        //{
        //    get { return _Jmeno; }
        //    set { _Jmeno = value; OnPropertyChanged("JmenoUzivatele"); }
        //}
        //string _Heslo = "";
        //public string HesloUzivatele
        //{
        //    get { return _Heslo; }
        //    set { _Heslo = value; OnPropertyChanged("HesloUzivatele"); }
        //}
        public string JmenoUzivatele { get; set; }
        public string HesloUzivatele { get; set; }
    }
    public class MnfUzivatel : ANotify
    {
        public int UserID { get; set; }
        public int premium { get; set; }
        public int premium_notification { get; set; }
        public int overcrowder { get; set; }
        public string LoginPaswCrypted { get; set; }
    }
    /// <summary>
    /// postava uzivatele
    /// </summary>
    public class Avatar : ANotify
    {
        public int AvatarID {get{return _AvatarID;}set{ _AvatarID = value; OnPropertyChanged("AvatarID");}}
        public string JmenoPostavy { get { return _JmenoPostavy; } set { _JmenoPostavy = value; OnPropertyChanged("_JmenoPostavy"); } }
        public string petnis_name {get{return _petnis_name; }set{ _petnis_name = value; OnPropertyChanged("petnis_name");}}
        public int sex {get{return _sex; }set{ _sex = value; OnPropertyChanged("sex");}}
        public int sexual_orientation {get{return _sexual_orientation; }set{ _sexual_orientation = value; OnPropertyChanged("sexual_orientation");}}
        public int skin_color {get{return _skin_color; }set{ _skin_color = value; OnPropertyChanged("skin_color");}}
        public int hair_color {get{return _hair_color; }set{ _hair_color = value; OnPropertyChanged("hair_color");}}
        public int eyes_color {get{return _eyes_color; }set{ _eyes_color = value; OnPropertyChanged("eyes_color");}}
        public int cloth_color1 {get{return _cloth_color1; }set{ _cloth_color1 = value; OnPropertyChanged("cloth_color1");}}
        public int cloth_color2 {get{return _cloth_color2; }set{ _cloth_color2 = value; OnPropertyChanged("cloth_color2");}}
        public int petnis_color {get{return _petnis_color; }set{ _petnis_color = value; OnPropertyChanged("petnis_color");}}
        public int torso {get{return _torso; }set{ _torso = value; OnPropertyChanged("torso");}}
        public int breast {get{return _breast; }set{ _breast = value; OnPropertyChanged("breast");}}
        public int nipples {get{return _nipples; }set{ _nipples = value; OnPropertyChanged("nipples");}}
        public int chest_hair {get{return _chest_hair; }set{ _chest_hair = value; OnPropertyChanged("chest_hair");}}
        public int pubic_hair {get{return _pubic_hair; }set{ _pubic_hair = value; OnPropertyChanged("pubic_hair");}}
        public int hair_style {get{return _hair_style; }set{ _hair_style = value; OnPropertyChanged("hair_style");}}
        public int jaw {get{return _jaw; }set{ _jaw = value; OnPropertyChanged("jaw");}}
        public int ears {get{return _ears; }set{ _ears = value; OnPropertyChanged("ears");}}
        public int nose {get{return _nose; }set{ _nose = value; OnPropertyChanged("nose");}}
        public int lips {get{return _lips; }set{ _lips = value; OnPropertyChanged("lips");}}
        public int eyes {get{return _eyes; }set{ _eyes = value; OnPropertyChanged("eyes");}}
        public int eye_brows {get{return _eye_brows; }set{ _eye_brows = value; OnPropertyChanged("eye_brows");}}
        public int moustaches {get{return _moustaches; }set{ _moustaches = value; OnPropertyChanged("moustaches");}}
        public int beard {get{return _beard; }set{ _beard = value; OnPropertyChanged("beard");}}
        public int outfit {get{return _outfit; }set{ _outfit = value; OnPropertyChanged("outfit");}}
        public string userCT {get{return _userCT; }set{ _userCT = value; OnPropertyChanged("userCT");}}
        public int exp_gross {get{return _exp_gross; }set{ _exp_gross = value; OnPropertyChanged("exp_gross");}}
        public int exp_available {get{return _exp_available; }set{ _exp_available = value; OnPropertyChanged("exp_available");}}
        public int deleted {get{return _deleted; }set{ _deleted = value; OnPropertyChanged("deleted");}}
        public int premium {get{return _premium; }set{ _premium = value; OnPropertyChanged("premium");}}
        public int hat {get{return _hat; }set{ _hat = value; OnPropertyChanged("hat");}}
        public int hat_color1 {get{return _hat_color1; }set{ _hat_color1 = value; OnPropertyChanged("hat_color1");}}
        public int hat_color2 {get{return _hat_color2; }set{ _hat_color2 = value; OnPropertyChanged("hat_color2");}}
        public int glasses {get{return _glasses; }set{ _glasses = value; OnPropertyChanged("glasses");}}
        public int glasses_color1 {get{return _glasses_color1; }set{ _glasses_color1 = value; OnPropertyChanged("glasses_color1");}}
        public int glasses_color2 {get{return _glasses_color2; }set{ _glasses_color2 = value; OnPropertyChanged("glasses_color2");}}
        public int hide_petnis {get{return _hide_petnis; }set{ _hide_petnis = value; OnPropertyChanged("hide_petnis");}}
        public int icon_id {get{return _icon_id; }set{ _icon_id = value; OnPropertyChanged("icon_id");}}

        private int _AvatarID = 0;
        private string _JmenoPostavy = "";
        private string _petnis_name = "";
        private int _sex = 0;
        private int _sexual_orientation = 0;
        private int _skin_color = 0;
        private int _hair_color = 0;
        private int _eyes_color = 0;
        private int _cloth_color1 = 0;
        private int _cloth_color2 = 0;
        private int _petnis_color = 0;
        private int _torso = 0;
        private int _breast = 0;
        private int _nipples = 0;
        private int _chest_hair = 0;
        private int _pubic_hair = 0;
        private int _hair_style = 0;
        private int _jaw = 0;
        private int _ears = 0;
        private int _nose = 0;
        private int _lips = 0;
        private int _eyes = 0;
        private int _eye_brows = 0;
        private int _moustaches = 0;
        private int _beard = 0;
        private int _outfit = 0;
        private string _userCT = "";
        private int _exp_gross = 0;
        private int _exp_available = 0;
        private int _deleted = 0;
        private int _premium = 0;
        private int _hat = 0;
        private int _hat_color1 = 0;
        private int _hat_color2 = 0;
        private int _glasses = 0;
        private int _glasses_color1 = 0;
        private int _glasses_color2 = 0;
        private int _hide_petnis = 0;
        private int _icon_id = 0;
        /// <summary>
        /// avatar_datas=2844033,YourSexDreams,2,1,2,8,3,26,33,2,3,2,2,1,6,2,1,5,3,2,2,2,4,2,252/201/176,0,0,-1,,0,0,1,0,0,1,0,0,0
        /// avatar_datas=2844033,YourSexDreams,2,1,2,8,3,26,33,2,3,2,2,1,6,2,1,5,3,2,2,2,4,2,176/252/250,225,225,-1,,0,0,1,0,0,1,0,0,0,1
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true = error</returns>
        public bool ParseAvatar(string s)
        {
            try
            {
                string[] ss = s.Split(',');
                try
                {
                    AvatarID = int.Parse(ss[0].Split('=')[1]);
                }
                catch
                {
                    AvatarID = int.Parse(ss[0]);
                }
                JmenoPostavy = ss[1];
                sex = int.Parse(ss[2]);
                sexual_orientation = int.Parse(ss[3]);
                skin_color = int.Parse(ss[4]);
                hair_color = int.Parse(ss[5]);
                eyes_color = int.Parse(ss[6]);
                cloth_color1 = int.Parse(ss[7]);
                cloth_color2 = int.Parse(ss[8]);
                torso = int.Parse(ss[9]);
                breast = int.Parse(ss[10]);
                nipples = int.Parse(ss[11]);
                chest_hair = int.Parse(ss[12]);
                pubic_hair = int.Parse(ss[13]);
                hair_style = int.Parse(ss[14]);
                jaw = int.Parse(ss[15]);
                ears = int.Parse(ss[16]);
                nose = int.Parse(ss[17]);
                lips = int.Parse(ss[18]);
                eyes = int.Parse(ss[19]);
                eye_brows = int.Parse(ss[20]);
                moustaches = int.Parse(ss[21]);
                beard = int.Parse(ss[22]);
                outfit = int.Parse(ss[23]);
                userCT = ss[24];
                exp_gross = int.Parse(ss[25]);
                exp_available = int.Parse(ss[26]);
                petnis_color = int.Parse(ss[27]);
                petnis_name = ss[28];
                deleted = int.Parse(ss[29]);
                premium = int.Parse(ss[30]);
                hat = int.Parse(ss[31]);
                hat_color1 = int.Parse(ss[32]);
                hat_color2 = int.Parse(ss[33]);
                glasses = int.Parse(ss[34]);
                glasses_color1 = int.Parse(ss[35]);
                glasses_color2 = int.Parse(ss[36]);
                hide_petnis = int.Parse(ss[37]);
                icon_id = int.Parse(ss[38]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
            return false;
        }

        public bool ParseAvatarCutted(string s)
        {
            try
            {
                string[] ss = s.Split(',');
                AvatarID = int.Parse(ss[0]);
                JmenoPostavy = ss[1];
                sex = int.Parse(ss[2]);
                sexual_orientation = int.Parse(ss[3]);
                skin_color = int.Parse(ss[4]);
                hair_color = int.Parse(ss[5]);
                eyes_color = int.Parse(ss[6]);
                cloth_color1 = int.Parse(ss[7]);
                cloth_color2 = int.Parse(ss[8]);
                torso = int.Parse(ss[9]);
                breast = int.Parse(ss[10]);
                nipples = int.Parse(ss[11]);
                chest_hair = int.Parse(ss[12]);
                pubic_hair = int.Parse(ss[13]);
                hair_style = int.Parse(ss[14]);
                jaw = int.Parse(ss[15]);
                ears = int.Parse(ss[16]);
                nose = int.Parse(ss[17]);
                lips = int.Parse(ss[18]);
                eyes = int.Parse(ss[19]);
                eye_brows = int.Parse(ss[20]);
                moustaches = int.Parse(ss[21]);
                beard = int.Parse(ss[22]);
                outfit = int.Parse(ss[23]);
                userCT = ss[24];
                exp_gross = int.Parse(ss[25]);
                exp_available = int.Parse(ss[26]);
                petnis_color = int.Parse(ss[27]);
                petnis_name = ss[28];
                deleted = int.Parse(ss[29]);
                premium = int.Parse(ss[30]);
                hat = int.Parse(ss[31]);
                hat_color1 = int.Parse(ss[32]);
                hat_color2 = int.Parse(ss[33]);
                glasses = int.Parse(ss[34]);
                glasses_color1 = int.Parse(ss[35]);
                glasses_color2 = int.Parse(ss[36]);
                hide_petnis = int.Parse(ss[37]);
                icon_id = int.Parse(ss[38]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// potkane psotavy
    /// </summary>
    public class MnfAvatar : Avatar
    {
        public MnfAvatar()
        {

        }
        public MnfAvatar(Avatar a)
        {
            AvatarID = a.AvatarID;
            JmenoPostavy = a.JmenoPostavy;
            sex = a.sex;
            sexual_orientation = a.sexual_orientation;
            skin_color = a.skin_color;
            hair_color = a.hair_color;
            eyes_color = a.eyes_color;
            cloth_color1 = a.cloth_color1;
            cloth_color2 = a.cloth_color2;
            torso = a.torso;
            breast = a.breast;
            nipples = a.nipples;
            chest_hair = a.chest_hair;
            pubic_hair = a.pubic_hair;
            hair_style = a.hair_style;
            jaw = a.jaw;
            ears = a.ears;
            nose = a.nose;
            lips = a.lips;
            eyes = a.eyes;
            eye_brows = a.eye_brows;
            moustaches = a.moustaches;
            beard = a.beard;
            outfit = a.outfit;
            userCT = a.userCT;
            exp_gross = a.exp_gross;
            exp_available = a.exp_available;
            petnis_color = a.petnis_color;
            petnis_name = a.petnis_name;
            deleted = a.deleted;
            premium = a.premium;
            hat = a.hat;
            hat_color1 = a.hat_color1;
            hat_color2 = a.hat_color2;
            glasses = a.glasses;
            glasses_color1 = a.glasses_color1;
            glasses_color2 = a.glasses_color2;
            hide_petnis = a.hide_petnis;
            icon_id = a.icon_id;
        }
        public int PoziceX { get; set; }
        public int PoziceY { get; set; }

        /// <summary>
        /// avatar data="8781987,edgomez,1,2,10,8,4,40,1,3,2,3,1,1,5,3,1,1,1,4,3,2,4,2,218/252/176,0,0,-1,,0,0,1,0,0,1,0,0,0,1" points="1126,743" />
        /// </summary>
        /// <param name="s"></param>
        public void ParseMnfAvatar(string s)
        {
            try
            {
                string[] ss = s.Split(' ');
                int i = 0;
                string[] data = ss[1].Replace("\"", "").Split(',');
                AvatarID = int.Parse(data[i++].Split('=')[1]);
                JmenoPostavy = data[i++];
                sex = int.Parse(data[i++]);
                sexual_orientation = int.Parse(data[i++]);
                skin_color = int.Parse(data[i++]);
                hair_color = int.Parse(data[i++]);
                eyes_color = int.Parse(data[i++]);
                cloth_color1 = int.Parse(data[i++]);
                cloth_color2 = int.Parse(data[i++]);
                torso = int.Parse(data[i++]);
                breast = int.Parse(data[i++]);
                nipples = int.Parse(data[i++]);
                chest_hair = int.Parse(data[i++]);
                pubic_hair = int.Parse(data[i++]);
                hair_style = int.Parse(data[i++]);
                jaw = int.Parse(data[i++]);
                ears = int.Parse(data[i++]);
                nose = int.Parse(data[i++]);
                lips = int.Parse(data[i++]);
                eyes = int.Parse(data[i++]);
                eye_brows = int.Parse(data[i++]);
                moustaches = int.Parse(data[i++]);
                beard = int.Parse(data[i++]);
                outfit = int.Parse(data[i++]);
                userCT = data[i++];
                exp_gross = int.Parse(data[i++]);
                exp_available = int.Parse(data[i++]);
                petnis_color = int.Parse(data[i++]);
                petnis_name = data[i++];
                deleted = int.Parse(data[i++]);
                premium = int.Parse(data[i++]);
                hat = int.Parse(data[i++]);
                hat_color1 = int.Parse(data[i++]);
                hat_color2 = int.Parse(data[i++]);
                glasses = int.Parse(data[i++]);
                glasses_color1 = int.Parse(data[i++]);
                glasses_color2 = int.Parse(data[i++]);
                hide_petnis = int.Parse(data[i++]);
                icon_id = int.Parse(data[i++]);

                string[] pozice = ss[2].Replace("\"", "").Split(',');
                PoziceX = int.Parse(pozice[0].Split('=')[1]);
                PoziceY = int.Parse(pozice[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    /// <summary>
    /// dostupne servery
    /// </summary>
    public class Server
    {
        public string JmenoServeru { get; set; }
        public string AdresaIP { get; set; }
        public int PocetMuzu { get; set; }
        public int PocetZen { get; set; }
        public int Kapacita { get; set; }
        public int top_socket { get; set; }
        public int area_socket { get; set; }
        public int game_socket { get; set; }
        public int chat_socket { get; set; }
        public int policy_socket { get; set; }

        /// <summary>
        /// name="Beaver Bash" ip="93.190.138.189" males="98" females="100" capacity="200" top_socket="2020" area_socket="2021" game_socket="2022" chat_socket="2023" policy_socket="843" />
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true = error</returns>
        public bool ParseServer(string s)
        {
            if (s == null || s == "") throw new Exception("Prazdny retezec");
            string[] ss = s.Split('=');
            if (ss.Length < 10) throw new Exception("Kratky retezec");
            JmenoServeru = ss[1].Split('\"')[1];
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
    }
    public class MnfServer:Server
    {
        public MnfServer()
        {

        }
        public MnfServer(Server s)
        {
            AdresaIP = s.AdresaIP;
            area_socket = s.area_socket;
            chat_socket = s.chat_socket;
            game_socket = s.game_socket;
            JmenoServeru = s.JmenoServeru;
            Kapacita = s.Kapacita;
            PocetMuzu = s.PocetMuzu;
            PocetZen = s.PocetZen;
            policy_socket = s.policy_socket;
            top_socket = s.top_socket;
        }
        public DateTime Server_time = DateTime.Now;
        public string Session_id = "";

        public object LockerTop = new object();
        public object LockerGame = new object();
        public object LockerArea = new object();
        public object LockerChat = new object();
        public object LockerPolicy = new object();
        public TcpClient TC_top = new TcpClient();
        public TcpClient TC_game = new TcpClient();
        public TcpClient TC_area = new TcpClient();
        public TcpClient TC_chat = new TcpClient();
        public TcpClient TC_policy = new TcpClient();

        public TcpClient ConnectTC(TcpClient tc, int port)
        {
            if (tc.Connected) tc.Close();
            tc = new TcpClient(AdresaIP, port);
            return tc;
        }
    }
    public class MnfPlayer
    {
        public MnfUzivatel Uzivatel;
        public MnfAvatar Avatar;
        public MnfServer Server;
    }
    public class MnfLocation:ANotify
    {
        string _JmenoLokace = "";
        public string JmenoLokace
        {
            get { return _JmenoLokace; }
            set
            {
                _JmenoLokace = value;
                OnPropertyChanged("JmenoLokace");
            }
        }
        public Point PortLokace;
        public long IdLokace = 0;
        public MnfLocation()
        {

        }
        public MnfLocation(string j, Point p, long id)
        {
            JmenoLokace = j;PortLokace = p;IdLokace = id;
        }
    }
}
