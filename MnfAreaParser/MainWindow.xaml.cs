using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MnfAreaParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            drawer.Init(@"C:\Users\Legomaniak\Desktop\MnfSwf\Area52 Bunny Apartments\mansion03.jpg");

            /*
            ////TODO: load jpg image
            //var uri = new Uri(@"C:\Users\Legomaniak\Desktop\MnfSwf\MnfSwfOld\Area27\images\1.jpg");
            //var btmp = new BitmapImage(uri);
            //obrazek.Source = btmp;
            //obrazek.Height = btmp.Height;
            //obrazek.Width = btmp.Width;
            drawer.MainPoint = new Point(12.95, -7.7);
            //create areas
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(449.2, 249.55), new Point(486.85, 249.55) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(527.65, 168.4), new Point(597.55, 235.55), new Point(633.35, 253.8), new Point(754.35, 252.8), new Point(786.1, 235.05), new Point(865.95, 166.4) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(848.95, 227.65), new Point(847.95, 399.6), new Point(765.95, 409.1), new Point(765.95, 443.05), new Point(944.7, 476.3), new Point(984.3, 476.3), new Point(1010.3, 429.6), new Point(1002.3, 410.1), new Point(957.2, 364.35), new Point(958.2, 311.65) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(1056.45, 248.8), new Point(1118.55, 217.65) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(1171.7, 200.05), new Point(1102.55, 283.8), new Point(1037.7, 410.1), new Point(1037.7, 443.6), new Point(1127.6, 457.6) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(1211.75, 550.35), new Point(1156.7, 655.15), new Point(1161.7, 679.5), new Point(1322.35, 837.3) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(589.6, 824.3), new Point(548.85, 786.65), new Point(579.85, 613.65), new Point(678.65, 549.15), new Point(1099.55, 550.35), new Point(1126.2, 679.95), new Point(1054.45, 697.3), new Point(975.2, 776.25), new Point(938.9, 793.65), new Point(820.9, 793.65), new Point(780.95, 776.65), new Point(703.1, 698.3) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(400.05, 785.95), new Point(304.8, 763.15), new Point(255.95, 823.3) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(87.45, 764.65) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(414.05, 299.8), new Point(454.05, 298.8), new Point(469.05, 329.8), new Point(457.05, 363.35), new Point(644.35, 383.85), new Point(729.65, 410.1), new Point(728.65, 443.6) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(1132.2, 731.15), new Point(1200, 712.3), new Point(1276.2, 741.7), new Point(1280.2, 795.65), new Point(1201, 820.3), new Point(1130.2, 786.25) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(1010.3, 798.65), new Point(1085.55, 767.65), new Point(1152.7, 810.3), new Point(1075.2, 852.15) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(199.5, 742.2), new Point(237.8, 613.7), new Point(328.8, 544.15), new Point(405.85, 593.65), new Point(297, 757) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(424.85, 576.15), new Point(478.55, 509.3), new Point(565.85, 528.65), new Point(518.65, 597.3) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(379.05, 660.15), new Point(447.05, 635.15), new Point(527.65, 657.15), new Point(530.65, 706.3), new Point(462.2, 742.15), new Point(380.05, 715.3) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(98.45, 523.65), new Point(129.2, 568.15), new Point(232.4, 569.15), new Point(305.3, 458.8) });
            drawer.AddPolygon();
            drawer.BodyPolygonu.AddRange(new Point[] { new Point(99.45, 763.65), new Point(118.2, 740.15), new Point(145.95, 749.65), new Point(130.2, 775.25) });
            drawer.AddPolygon();

            var textFile = File.ReadAllText(@"C:\Users\Legomaniak\Desktop\Ribs.txt");
            var s = textFile.Replace(")", "").Replace(";\r\n", "").Split(',');
            int a = -1, b = -1;
            for (int i = 0; i < s.Length; i++)
            {
                var ss = s[i].Split('(');
                if (s[i].Contains("Array") && s[i].Contains("WalkRib"))
                {
                    //drawer.Ribs.Add(new List<WalkRib>());
                    a++;
                    //drawer.Ribs[a].Add(new WalkRib(double.Parse(ss.Last().Replace('.', ','))));
                    b = 0;
                }
                else if (s[i].Contains("Array"))
                {
                    drawer.Ribs[a][b].Pole = ss.Last();
                    while (!s[i + 1].Contains("Array") && !s[i + 1].Contains("WalkRib"))
                    {
                        drawer.Ribs[a][b].Pole += "," + s[++i];
                    }
                }
                else if (s[i].Contains("WalkRib"))
                {
                    //drawer.Ribs[a].Add(new WalkRib(double.Parse(ss.Last().Replace('.', ','))));
                    b++;
                }
            }
            */
        }
    }
}
