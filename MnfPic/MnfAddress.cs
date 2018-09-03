using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnfPic
{
    public static class MnfAddress
    {
        public static string SiteMain = "http://www.mnfclub.com/";
        public static string SiteSWF() { return "http://www.mnfclub.com/swf/"; }
        public static string SiteSWF(string s) { return SiteSWF() + s; }
        public static string SiteMuseum = "files/museum/";
        public static string SiteLogin = "login.php";
        public static string SiteAvatar = "avatar_list.php";
        public static string SiteBrothel = "brothel_checkup.php";
        public static string SiteServer = "servers_list.php";
        public static string SiteActive = "activate_avatar.php";
        public static string SiteArea(int i) { return "area" + i + ".php"; }

    }
}
