using Nini.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnfPic
{
    public abstract class Hodnota
    {
        public string Sekce = "Debug";
        public string Jmeno = "TestovaciHodnota";
        public object Value = 0;
        public abstract void Refresh(Nini.Config.IConfig config);
    }
    public class HodnotaInt : Hodnota
    {
        public override void Refresh(IConfig config)
        {
            Value = config.GetInt(Jmeno);
        }
    }
    public class HodnotaString : Hodnota
    {
        public override void Refresh(IConfig config)
        {
            Value = config.GetString(Jmeno);
        }
    }
    public class HodnotaBool : Hodnota
    {
        public override void Refresh(IConfig config)
        {
            Value = config.GetBoolean(Jmeno);
        }
    }
    public class HodnotaDouble : Hodnota
    {
        public override void Refresh(IConfig config)
        {
            Value = config.GetDouble(Jmeno);
        }
    }

}
