using App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mnf
{
    public static class NastaveniMnf
    {
        static MujInitSoubor Ini = new MujInitSoubor("NastaveniMnf");

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

        static Hodnota _UserFile = new HodnotaString() { Sekce = "Common", Jmeno = "UserFile", Value = "Users.txt" };
        public static string UserFile
        {
            get { return (string)_UserFile.Value; }
            set
            {
                if ((string)_UserFile.Value != value)
                {
                    _UserFile.Value = value;
                    Save();
                }
            }
        }
        
        static Hodnota _SaveImages = new HodnotaBool() { Sekce = "Lockers", Jmeno = "SaveImages", Value = true };
        public static bool SaveImages
        {
            get { return (bool)_SaveImages.Value; }
            set
            {
                if ((bool)_SaveImages.Value != value)
                {
                    _SaveImages.Value = value;
                    Save();
                }
            }
        }
        
        static Hodnota _SaveStrangers = new HodnotaBool() { Sekce = "Lockers", Jmeno = "SaveStrangers", Value = false };
        public static bool SaveStrangers
        {
            get { return (bool)_SaveStrangers.Value; }
            set
            {
                if ((bool)_SaveStrangers.Value != value)
                {
                    _SaveStrangers.Value = value;
                    Save();
                }
            }
        }

        static Hodnota _SaveFriendList = new HodnotaBool() { Sekce = "Lockers", Jmeno = "SaveFriendList", Value = true };
        public static bool SaveFriendList
        {
            get { return (bool)_SaveFriendList.Value; }
            set
            {
                if ((bool)_SaveFriendList.Value != value)
                {
                    _SaveFriendList.Value = value;
                    Save();
                }
            }
        }

        static Hodnota _SaveFolderBG = new HodnotaString() { Sekce = "Folders", Jmeno = "SaveFolderBG", Value = false };
        public static string SaveFolderBG
        {
            get { return (string)_SaveFolderBG.Value; }
            set
            {
                if ((string)_SaveFolderBG.Value != value)
                {
                    _SaveFolderBG.Value = value;
                    Save();
                }
            }
        }

        static Hodnota _SaveFileUsers = new HodnotaString() { Sekce = "Folders", Jmeno = "SaveFileUsers", Value = false };
        public static string SaveFileUsers
        {
            get { return (string)_SaveFileUsers.Value; }
            set
            {
                if ((string)_SaveFileUsers.Value != value)
                {
                    _SaveFileUsers.Value = value;
                    Save();
                }
            }
        }

        static Hodnota _SaveFolderPic = new HodnotaString() { Sekce = "Folders", Jmeno = "SaveFolderPic", Value = false };
        public static string SaveFolderPic
        {
            get { return (string)_SaveFolderPic.Value; }
            set
            {
                if ((string)_SaveFolderPic.Value != value)
                {
                    _SaveFolderPic.Value = value;
                    Save();
                }
            }
        }


        /// <summary>
        /// PRIDAT VSECHNY PROMENE .. pro ukladani a nacitani
        /// </summary>
        static List<Hodnota> Hodnoty = new List<Hodnota>() { _MainFile, _UserFile };

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