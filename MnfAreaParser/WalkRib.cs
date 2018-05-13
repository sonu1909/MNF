using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MnfAreaParser
{
    public class WalkRib
    {
        public int ID { get; set; }
        public Point P { get; set; }
        //public double D { get; set; }
        //public string Pole { get; set; }
        public string NextWalkRib { get; set; }
    }
}
