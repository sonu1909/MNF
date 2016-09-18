using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnfPic
{
    /// <summary>
    /// mela by byt abstract, ale neni :D
    /// </summary>
    public class SouborIni
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jmeno">Nazev souboru</param>
        public SouborIni(string jmeno)
        {
            Ini = new MujInitSoubor(jmeno);
            //Hodnoty = new List<Hodnota>() { _MojeHodnota, };
            Load();
        }
        /// <summary>
        /// Seznam hodnot
        /// </summary>
        public List<Hodnota> Hodnoty;
        /// <summary>
        /// soubor hodnot
        /// </summary>
        public MujInitSoubor Ini;
        /// <summary>
        /// Uloží Hodnoty
        /// </summary>
        public void Save()
        {
            if (Hodnoty == null) return;
            Nini.Config.IConfigSource source = Ini.InitSoubor();
            source.Configs.Clear();
            foreach (Hodnota h in Hodnoty)
            {
                Nini.Config.IConfig config = null;
                if (!source.Configs.Contains(h.Sekce)) config = source.AddConfig(h.Sekce);
                else config = source.Configs[h.Sekce];
                if (config != null)
                {
                    config.Set(h.Jmeno, h.Value);
                }

            }
            source.Save();
        }
        /// <summary>
        /// Nacte hodnoty ze souboru
        /// </summary>
        public void Load()
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
        /*//Ukazka pouziti
        Hodnota _MojeHodnota = new HodnotaInt() { Sekce = "Test", Jmeno = "TestovaciHodnota", Value = 0 };
        public int MojeHodnota
        {
            get { return (int)_MojeHodnota.Value; }
            set
            {
                _MojeHodnota.Value = value;
                Save();
            }
        }
        */
    }
}