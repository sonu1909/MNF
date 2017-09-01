using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class TvrobaInit
    {
        string pripona = ".ini";
        public string VratInit(string jmeno)
        {
            string IniPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(IniPath)) return null;
            string s = Path.Combine(IniPath, jmeno + pripona);

            //return File.Exists(s) ? s : null;
            if (!File.Exists(s)) File.Create(s).Close();
            return s;
        }
        public string VratInit(string jmeno, Nini.Config.IConfigSource defaultConfigSource)
        {
            if (defaultConfigSource == null) return VratInit(jmeno);
            string IniPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(IniPath)) return null;
            string s = Path.Combine(IniPath, jmeno + pripona);

            if (!File.Exists(s))
            {
                File.Create(s).Close();
                Nini.Config.IConfigSource source = new Nini.Config.IniConfigSource(s);
                source.Merge(defaultConfigSource);
                source.Save();
            }
            return s;
        }
    }
    public class MujInitSoubor
    {
        public MujInitSoubor(string jmeno)
        {
            Jmeno = jmeno;
            //defaultConfigSource = new Nini.Config.IniConfigSource();
            //Nini.Config.IConfig config = defaultConfigSource.AddConfig("default");
            //config.Set("String", "mujString");
        }
        public string Jmeno;
        TvrobaInit ti = new TvrobaInit();
        public Nini.Config.IConfigSource defaultConfigSource;
        public Nini.Config.IConfigSource InitSoubor()
        {
            string s = ti.VratInit(Jmeno, defaultConfigSource);
            return s == null ? null : new Nini.Config.IniConfigSource(s);
        }
    }
}
