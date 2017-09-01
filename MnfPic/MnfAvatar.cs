using App;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MnfPic
{
    public class MnfAvatar:ANotify
    {
        public int AvatarID = 0;

        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
        public string petnis_name = "";
        public int sex = 1;
        public int sexual_orientation = 2;
        public int skin_color = 1;
        public int hair_color = 4;
        public int eyes_color = 4;
        public int cloth_color1 = 1;
        public int cloth_color2 = 10;
        public int petnis_color = -1;
        public int torso = 2;
        public int breast = 2;
        public int nipples = 2;
        public int chest_hair = 2;
        public int pubic_hair = 1;
        public int hair_style = 1;
        public int jaw = 2;
        public int ears = 1;
        public int nose = 1;
        public int lips = 1;
        public int eyes = 1;
        public int eye_brows = 1;
        public int moustaches = 1;
        public int beard = 1;
        public int outfit = 2;
        public string userCT ="";
        public int exp_gross = 0;
        public int exp_available = 0;
        public int deleted = 0;
        public int premium = 0;
        public int hat = 1;
        public int hat_color1 = 1;
        public int hat_color2 = 10;
        public int glasses = 1;
        public int glasses_color1 = 1;
        public int glasses_color2 = 10;
        public int hide_petnis = 0;
        public int novaHodnota = 0;
        public MnfUser User;
        
        public List<MnfServer> Servery = new List<MnfServer>();

        public MnfAvatar(MnfUser user) { User = user; }

        WebClient wc = new WebClient();
        /// <summary>
        /// avatar_datas=2844033,YourSexDreams,2,1,2,8,3,26,33,2,3,2,2,1,6,2,1,5,3,2,2,2,4,2,252/201/176,0,0,-1,,0,0,1,0,0,1,0,0,0
        /// avatar_datas=2844033,YourSexDreams,2,1,2,8,3,26,33,2,3,2,2,1,6,2,1,5,3,2,2,2,4,2,176/252/250,225,225,-1,,0,0,1,0,0,1,0,0,0,1
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true = error</returns>
        public bool StringParse(string s)
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
                Name = ss[1];
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
            catch(Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
            return false;
        }


        public void VyberAvatara()
        {
            string s;
            //kontrola brothel
            var data = new NameValueCollection();
            data["avatar_id"] = AvatarID.ToString();
            data["pass"] = User.LoginPaswCrypted;
            data["user_id"] = User.UserID.ToString();
            var response = wc.UploadValues(MnfAddress.SiteMain + MnfAddress.SiteBrothel, "POST", data);

            s = Encoding.UTF8.GetString(response, 0, response.Length);
            string[] ss = s.Split('&');
            if (ss[1].Split('=')[1] != "not_working") { throw new Exception("pracujes v brothelu!!\n" + s); }
        }

        public void NactiServery()
        {
            string s;
            ////Zisk Serveru
            var data = new NameValueCollection();
            data["color"] = userCT;
            data["avatar_id"] = AvatarID.ToString();
            data["pass"] = User.LoginPaswCrypted;
            data["user_id"] = User.UserID.ToString();
            var response = wc.UploadValues(MnfAddress.SiteMain + MnfAddress.SiteServer, "POST", data);

            s = Encoding.UTF8.GetString(response, 0, response.Length);
            string[] ss = s.Replace("<server ", ";").Split(';');
            for (int i = 1; i < ss.Length; i++)
            {
                MnfServer ms = new MnfServer(this);
                if (ms.StringParse(ss[i])) { throw new Exception("bad response\n" + s); }
                Servery.Add(ms);
            }
        }
    }
}
