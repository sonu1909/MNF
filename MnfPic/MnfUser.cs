using App;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MnfPic
{
    public class MnfUser:ANotify
    {
        private int _UserID = 0;

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; OnPropertyChanged("UserID"); }
        }

        public int premium = 0;
        public int premium_notification=0;
        public int overcrowder = 0;
        public string LoginPaswCrypted = "";
        
        public List<MnfAvatar> Avatary = new List<MnfAvatar>();

        
        public MnfUser(string loginPaswCrypted)
        {
            LoginPaswCrypted = loginPaswCrypted;
        }
        
        public bool StringParse(string s)
        {
            try
            {
                string[] ss = s.Split('&');
                //errors = int.Parse(ss[1].Split('=')[1]);
                UserID = int.Parse(ss[2].Split('=')[1]);
                premium = int.Parse(ss[3].Split('=')[1]);
                premium_notification = int.Parse(ss[4].Split('=')[1]);
                overcrowder = int.Parse(ss[5].Split('=')[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
            return false;
        }

        WebClient wc = new WebClient();
        public void NactiAvatary()
        {
            string s;
            ////Zisk Avatara
            var data = new NameValueCollection();
            data["pass"] = LoginPaswCrypted;
            data["user_id"] = UserID.ToString();//nebo user%5Fid

            var response = wc.UploadValues(MnfAddress.SiteMain + MnfAddress.SiteAvatar, "POST", data);
            s = Encoding.UTF8.GetString(response, 0, response.Length);
            string[] ss = s.Split('&')[1].Split(';');
            //if (ss.Length < 5) { MessageBox.Show("bad response\n" + s); return true; }
            for (int i = 0; i < ss.Length; i++)
            {
                MnfAvatar ma = new MnfAvatar(this);
                if (ma.StringParse(ss[i])) {throw new Exception("bad response\n" + s); }
                else Avatary.Add(ma);
            }
        }


    }
}
