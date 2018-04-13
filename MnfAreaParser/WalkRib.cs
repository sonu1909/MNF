using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnfAreaParser
{
    public class WalkRib
    {
        public WalkRib()
        {
            D = 0;
            Pole = "";
        }
        public WalkRib(double d)
        {
            D = d;
            Pole = "";
        }
        public WalkRib(double d,string pole)
        {
            D = d;
            Pole = pole;
        }
        public double D { get; set; }
        public string Pole { get; set; }

    }
}
