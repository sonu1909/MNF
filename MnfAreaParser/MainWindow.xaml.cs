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
        }

        private void ClickLoadPoints(object sender, RoutedEventArgs e)
        {
            LoadWindow lw = new LoadWindow();
            if (lw.ShowDialog().Value)
            {
                drawer.stackPanelN.Children.Clear();
                drawer.Ribs.Clear();
                drawer.BodyPolygonu.Clear();
                drawer.listBox.Items.Clear();
                drawer.stackPanel.Children.Clear();
                var textFile = lw.TB.Text.Split('\n');
                foreach (var t in textFile)
                {
                    if (t.Contains("walk_manager.AddPointGroup"))
                    {
                        var s = t.Replace(")", "").Replace(";\r", "").Split(',');
                        Point bod = new Point();
                        for (int i = 0; i < s.Length; i++)
                        {
                            var ss = s[i].Split('(');
                            if (s[i].Contains("Array") && s[i].Contains("WalkPoint"))
                            {
                                drawer.AddPolygon();
                                bod = new Point();
                                bod.X = double.Parse(ss.Last().Replace('.', ','));
                            }
                            else if (s[i].Contains("WalkPoint"))
                            {
                                bod = new Point();
                                bod.X = double.Parse(ss.Last().Replace('.', ','));
                            }
                            else
                            {
                                bod.Y = double.Parse(ss.Last().Replace('.', ','));
                                drawer.BodyPolygonu.Add(bod);
                            }
                        }
                        drawer.AddPolygon();
                    }
                    //        else if (t.Contains("walk_manager"))
                    //        {
                    //            var s = t.Replace(")", "").Replace(";\r", "").Split(',');
                    //            int a = -1, b = -1;
                    //            for (int i = 0; i < s.Length; i++)
                    //            {
                    //                var ss = s[i].Split('(');
                    //                if (s[i].Contains("Array") && s[i].Contains("WalkRib"))
                    //                {
                    //                    //drawer.Ribs.Add(new List<WalkRib>());
                    //                    a++;
                    //                    b = 0;
                    //                    drawer.Ribs[a][b].D = double.Parse(ss.Last().Replace('.', ','));
                    //                    //drawer.Ribs[a].Add(new WalkRib(double.Parse(ss.Last().Replace('.', ','))));
                    //                }
                    //                else if (s[i].Contains("Array"))
                    //                {
                    //                    drawer.Ribs[a][b].Pole = ss.Last();
                    //                    while (!s[i + 1].Contains("Array") && !s[i + 1].Contains("WalkRib"))
                    //                    {
                    //                        drawer.Ribs[a][b].Pole += "," + s[++i];
                    //                    }
                    //                }
                    //                else if (s[i].Contains("WalkRib"))
                    //                {
                    //                    drawer.Ribs[a][b].D = double.Parse(ss.Last().Replace('.', ','));
                    //                    //drawer.Ribs[a].Add(new WalkRib(double.Parse(ss.Last().Replace('.', ','))));
                    //                    b++;
                    //                }
                    //            }
                    //        }
                    //        else Console.WriteLine("Bad Line");
                }
            }
            drawer.CreateGraphics();
        }

        private void ClickLoadBG(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                drawer.Init(ofd.FileName);
            }
        }
    }
}
