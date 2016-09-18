using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnfPic
{
    public static class NastaveniMnfPic
    {
        static MujInitSoubor Ini = new MujInitSoubor("NastaveniMnfPic");

        static Hodnota _MainFile = new HodnotaString() { Sekce = "Common", Jmeno = "MainFile", Value = "" };
        public static string MainFile
        {
            get { return (string)_MainFile.Value; }
            set
            {
                if ((string)_MainFile.Value != value)
                {
                    _MainFile.Value = value;
                    Save();
                }
            }
        }

        static Hodnota _LoginUser = new HodnotaString() { Sekce = "Common", Jmeno = "LoginUser", Value = "" };
        public static string LoginUser
        {
            get { return (string)_LoginUser.Value; }
            set
            {
                if ((string)_LoginUser.Value != value)
                {
                    _LoginUser.Value = value;
                    Save();
                }
            }
        }

        static Hodnota _LoginPaswCrypted = new HodnotaString() { Sekce = "Common", Jmeno = "LoginPaswCrypted", Value = "" };
        public static string LoginPaswCrypted
        {
            get { return (string)_LoginPaswCrypted.Value; }
            set
            {
                if ((string)_LoginPaswCrypted.Value != value)
                {
                    _LoginPaswCrypted.Value = value;
                    Save();
                }
            }
        }








        /// <summary>
        /// PRIDAT VSECHNY PROMENE .. pro ukladani a nacitani
        /// </summary>
        static List<Hodnota> Hodnoty = new List<Hodnota>() { _MainFile, _LoginUser, _LoginPaswCrypted };

        /// <summary>
        /// Uloží Hodnoty
        /// </summary>
        public static void Save()
        {
            if (Hodnoty == null) return;
            Nini.Config.IConfigSource source = Ini.InitSoubor();
            source.Configs.Clear();
            foreach (Hodnota h in Hodnoty)
            {
                if (h != null)
                {
                    Nini.Config.IConfig config = source.Configs[h.Sekce];
                    if (config == null) config = source.AddConfig(h.Sekce);
                    if (config != null)
                    {
                        config.Set(h.Jmeno, h.Value);
                    }
                }
            }
            source.Save();
        }
        /// <summary>
        /// Nacte hodnoty ze souboru
        /// </summary>
        public static void Load()
        {
            if (Hodnoty == null) return;
            //nacte hodnoty
            Nini.Config.IConfigSource source = Ini.InitSoubor();
            foreach (Hodnota h in Hodnoty)
            {
                Nini.Config.IConfig config = source.Configs[h.Sekce];
                if (config == null) config = source.AddConfig(h.Sekce);
                if (config != null)
                {
                    if (config.Contains(h.Jmeno))
                    {
                        h.Refresh(config);
                    }
                    else
                    {
                        config.Set(h.Jmeno, h.Value);
                    }
                }

            }
        }
    }
}