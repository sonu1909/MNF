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
        public int AvatarID { get; set; }
        public string JmenoPostavy { get; set; }
        public string petnis_name { get; set; }
        public int sex { get; set; }
        public int sexual_orientation { get; set; }
        public int skin_color { get; set; }
        public int hair_color { get; set; }
        public int eyes_color { get; set; }
        public int cloth_color1 { get; set; }
        public int cloth_color2 { get; set; }
        public int petnis_color { get; set; }
        public int torso { get; set; }
        public int breast { get; set; }
        public int nipples { get; set; }
        public int chest_hair { get; set; }
        public int pubic_hair { get; set; }
        public int hair_style { get; set; }
        public int jaw { get; set; }
        public int ears { get; set; }
        public int nose { get; set; }
        public int lips { get; set; }
        public int eyes { get; set; }
        public int eye_brows { get; set; }
        public int moustaches { get; set; }
        public int beard { get; set; }
        public int outfit { get; set; }
        public string userCT { get; set; }
        public int exp_gross { get; set; }
        public int exp_available { get; set; }
        public int deleted { get; set; }
        public int premium { get; set; }
        public int hat { get; set; }
        public int hat_color1 { get; set; }
        public int hat_color2 { get; set; }
        public int glasses { get; set; }
        public int glasses_color1 { get; set; }
        public int glasses_color2 { get; set; }
        public int hide_petnis { get; set; }
        public int novaHodnota { get; set; }

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
                novaHodnota = int.Parse(ss[38]);
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
            novaHodnota = a.novaHodnota;
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
                novaHodnota = int.Parse(data[i++]);

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
    public class MnfLocation
    {
        public string JmenoLokace = "";
        public Point PortLokace;
        public int IdLokace = 0;
        public MnfLocation()
        {

        }
        public MnfLocation(string j, Point p, int id)
        {
            JmenoLokace = j;PortLokace = p;IdLokace = id;
        }
    }
}
