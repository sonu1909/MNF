using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nini;

namespace MnfServer
{
    public static class Nastaveni
    {
        public static void Init()
        {
            string FileName = "Init.ini";
            if (!File.Exists(FileName)) throw new Exception("Ini file not found!");

            Nini.Config.IConfigSource source = new Nini.Config.IniConfigSource(FileName);

            Nini.Config.IConfig config = source.Configs["MySQL"];
            DBname = config.Get("DBname");
            DBuser = config.Get("DBuser");
            DBpassw = config.Get("DBpassw");
            DBserver = config.Get("DBserver");

            config = source.Configs["Server"];
            PortIP = config.GetInt("PortIP");
            AdressIP = config.Get("AdressIP");
        }
        public static string DBserver, DBname, DBuser, DBpassw, AdressIP;
        public static int PortIP;

    }
}
