using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnfPic
{
    public static class MnfAddress
    {
        public static string SiteMain() { return "https://www.mnfclub.com/"; }
        public static string SiteMain(string s) { return SiteMain() + s; }
        public static string SiteSWF() { return "https://www.mnfclub.com/swf/"; }
        public static string SiteSWF(string s) { return SiteSWF() + s; }
        public static string SiteMuseum = "files/museum/";
        public static string SiteLogin = "login.php";
        public static string SiteAvatar = "avatar_list.php";
        public static string SiteBrothel = "brothel_checkup.php";
        public static string SiteServer = "servers_list.php";
        public static string SiteActive = "activate_avatar.php";
        public static string SiteArea(int i) { return "area" + i + ".swf?1." + Properties.Settings.Default.Verze; }
        public static string SiteBG(string s) { return SiteMain() + "files/property_bgs/bg" + s + ".jpg?1." + Properties.Settings.Default.Verze; }

    }
}
