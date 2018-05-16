using System;
using System.Collections.Generic;
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
    /// Interaction logic for Drawer.xaml
    /// </summary>
    public partial class Drawer : UserControl
    {
        public Drawer()
        {
            InitializeComponent();
        }
        public List<Point> BodyPolygonu = new List<Point>();
        public List<WalkRib> Ribs = new List<WalkRib>();
        public Point MainPoint = new Point();
        public bool NewPolygonFlag = false;
        public void Init(string cesta)
        {
            try
            {
                var uri = new Uri(cesta);
                var btmp = new BitmapImage(uri);
                obrazek.Source = btmp;
                obrazek.Height = btmp.Height;
                obrazek.Width = btmp.Width;
            }
            catch { }
        }

        private void Click_Rem(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex >= 0)
            {
                var l = (int)listBox.SelectedItem;
                Ribs.RemoveAll(x => x.ID == l);
                listBox.Items.RemoveAt(l);
                CreateGraphics();
                CreateRibs();
            }
        }

        private void Click_Add(object sender, RoutedEventArgs e)
        {
            NewPolygonFlag = !NewPolygonFlag;
            BodyPolygonu.Clear();
        }

        private void mainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (NewPolygonFlag)
            {
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    AddPolygon();
                }
                else BodyPolygonu.Add(new Point(e.GetPosition(mainGrid).X - MainPoint.X, e.GetPosition(mainGrid).Y - MainPoint.Y));
            }
        }
        public void AddPolygon()
        {
            if (BodyPolygonu.Count > 0)
            {
                int ID = 1;
                if (Ribs.Count > 0) ID = (from f in Ribs select f.ID).Max() + 1;
                foreach (Point p in BodyPolygonu)
                {
                    Ribs.Add(new WalkRib()
                    {
                        ID = ID,
                        P = p,
                    });
                }
                listBox.Items.Add(ID);
                CreateGraphics();
                BodyPolygonu.Clear();
                CreateRibs();
            }
        }
        public void CreateGraphics()
        {
            mainGrid.Children.RemoveRange(1, mainGrid.Children.Count - 1);
            if (Ribs.Count < 1) return;
            var ID = (from f in Ribs select f.ID).Max() + 1;
            for (int id = 0; id < ID; id++)
            {
                var i = (from f in Ribs where f.ID == id select f.P).ToArray();
                if (i.Count() > 2)
                {
                    mainGrid.Children.Add(new Polygon()
                    {
                        Points = new PointCollection((from f in i select new Point(f.X + MainPoint.X, f.Y + MainPoint.Y)).ToArray()),
                        Fill = Brushes.Blue,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                    });
                    double x = 0, y = 0;
                    foreach (var j in i)
                    {
                        x += j.X;
                        y += j.Y;
                    }
                    x /= i.Count();
                    y /= i.Count();
                    mainGrid.Children.Add(new Label()
                    {
                        Content = id,
                        Margin = new Thickness(x + MainPoint.X, y + MainPoint.Y + 10, 0, 0),
                        Foreground = Brushes.Black,
                        Background = Brushes.White,
                    });
                }
                else if (i.Count() > 1)
                    mainGrid.Children.Add(new Line()
                    {
                        X1 = i[0].X + MainPoint.X,
                        X2 = i[1].X + MainPoint.X,
                        Y1 = i[0].Y + MainPoint.Y,
                        Y2 = i[1].Y + MainPoint.Y,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 5,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                    });
                else if (i.Count() == 1)
                {
                    foreach (var f in i)
                    {
                        mainGrid.Children.Add(new Ellipse()
                        {
                            Width = 10,
                            Height = 10,
                            Margin = new Thickness(f.X + MainPoint.X, f.Y + MainPoint.Y, 0, 0),
                            Fill = Brushes.Blue,
                            Stroke = Brushes.Blue,
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left,
                        });
                    }
                }
            }

            int pozice = 0;
            foreach (var i in Ribs)
            {
                mainGrid.Children.Add(new Label()
                {
                    Content = pozice++,
                    Margin = new Thickness(i.P.X + MainPoint.X, i.P.Y + MainPoint.Y, 0, 0),
                    Foreground = Brushes.Red,
                    Background = Brushes.White,
                });
            }
        }
        public void CreateRibs()
        {
            stackPanel.Children.Clear();
            stackPanelN.Children.Clear();
            for (int i = 0; i < Ribs.Count; i++)
            {
                Label l = new Label() { Content = i, Height = 25, };
                stackPanel.Children.Add(l);

                var tb = new TextBox() { Height = 25, };
                Binding b = new Binding("NextWalkRib");
                b.Source = Ribs[i];
                b.Mode = BindingMode.TwoWay;
                tb.SetBinding(TextBox.TextProperty, b);
                stackPanelN.Children.Add(tb);
            }
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBox.SelectedIndex < 0) return;
            StackPanel sp1 = new StackPanel();
            StackPanel sp2 = new StackPanel();
            int ID = (int)listBox.SelectedItem;
            foreach (var i in Ribs)
            {
                if (i.ID == ID)
                {
                    var tb = new TextBox() { TextAlignment = TextAlignment.Center };
                    Binding b = new Binding("P.X");//nelze prepsat (vlastni Point?)
                    b.Source = i;
                    b.Mode = BindingMode.TwoWay;
                    tb.SetBinding(TextBox.TextProperty, b);
                    sp1.Children.Add(tb);

                    tb = new TextBox() { TextAlignment = TextAlignment.Center };
                    b = new Binding("P.Y");//nelze prepsat
                    b.Source = i;
                    b.Mode = BindingMode.TwoWay;
                    tb.SetBinding(TextBox.TextProperty, b);
                    sp2.Children.Add(tb);
                }
            }
            var g = new Grid();
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.Children.Add(sp1);
            g.Children.Add(sp2);
            Grid.SetColumn(sp2, 1);
            var w = new Window() { Content = g };
            w.ShowDialog();
            CreateGraphics();
        }
        double[,] Delky;
        private void Click_Generate(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                var ID = (from f in Ribs select f.ID).Max() + 1;
                for (int id = 0; id < ID; id++)
                {
                    var polygon = (from f in Ribs where f.ID == id select f).ToArray();
                    if (polygon.Length > 0)
                    {
                        //walk_manager.AddPointGroup(new Array(new WalkPoint(449.2, 249.55), new WalkPoint(486.85, 249.55)));
                        sb.Append("walk_manager.AddPointGroup(new Array(");
                        sb.Append("new WalkPoint(" + UpravCislo(polygon[0].P.X) + ", " + UpravCislo(polygon[0].P.Y) + ")");
                        for (int i = 1; i < polygon.Length; i++)
                        {
                            sb.Append(", new WalkPoint(" + UpravCislo(polygon[i].P.X) + ", " + UpravCislo(polygon[i].P.Y) + ")");
                        }
                        sb.AppendLine("));");
                    }
                }
                MaxI = Ribs.Count;
                Delky = new double[MaxI, MaxI];
                for (int i = 0; i < MaxI; i++)
                {
                    for (int j = 0; j < MaxI; j++)
                    {
                        Delky[i, j] = Math.Sqrt(Math.Pow(Ribs[i].P.X - Ribs[j].P.X, 2) + Math.Pow(Ribs[i].P.Y - Ribs[j].P.Y, 2));
                    }
                }
                var Pole = new string[MaxI, MaxI];
                Parallel.For(0, MaxI, i =>
                 {
                     for (int j = i; j < MaxI; j++)
                     {
                         if (i == j) Pole[i, j] = "";
                         else
                         {
                             var cesta = GeneratePath(i, j);
                             Pole[i, j] = cesta;
                             Pole[j, i] = new string(cesta.ToCharArray().Reverse().ToArray());
                         }
                     }
                 });
                sb.Append("walk_manager.ribs = new Array(");
                sb.Append("new Array(");
                sb.Append("new WalkRib(" + UpravCislo(GetDelka(Ribs[0], Ribs[0])));
                if (Pole[0,0] != "") sb.Append(",new Array(" + Pole[0, 0] + ")");
                sb.Append(")");
                for (int j = 1; j < MaxI; j++)
                {
                    sb.Append(",new WalkRib(" + UpravCislo(GetDelka(Ribs[0], Ribs[j])));
                    if (Pole[0, j] != "") sb.Append(",new Array(" + Pole[0, j] + ")");
                    sb.Append(")");
                }
                sb.Append(")");
                for (int i = 1; i < Ribs.Count; i++)
                {
                    sb.Append(",new Array(");
                    sb.Append("new WalkRib(" + UpravCislo(GetDelka(Ribs[i], Ribs[0])));
                    if (Pole[i, 0] != "") sb.Append(",new Array(" + Pole[i, 0] + ")");
                    sb.Append(")");
                    for (int j = 1; j < MaxI; j++)
                    {
                        sb.Append(",new WalkRib(" + UpravCislo(GetDelka(Ribs[i], Ribs[j])));
                        if (Pole[i, j] != "") sb.Append(",new Array(" + Pole[i, j] + ")");
                        sb.Append(")");
                    }
                    sb.Append(")");
                }
                sb.AppendLine(");");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(sb.ToString());
        }
        public string UpravCislo(double d)
        {
            return d.ToString("0.00").Replace(',', '.');
        }
        public double GetDelka(WalkRib wr1, WalkRib wr2)
        {
            return Math.Round(Math.Sqrt(Math.Pow(wr1.P.X - wr2.P.X, 2) + Math.Pow(wr1.P.Y - wr2.P.Y, 2)), 2);
        }
        public string GeneratePath(int FromI, int ToI)
        {
            var NejkratsiCesta = new List<string>();
            SearchPath(NejkratsiCesta, "", FromI, -1, ToI, 0);
            var Cesty = new List<int[]>();
            foreach (var s in NejkratsiCesta)
            {
                Cesty.Add(CreateCesta(FromI, ToI, s));
            }
            var DelkyCesty = new List<double>();
            foreach (var s in Cesty)
            {
                DelkyCesty.Add(DelkaCesty(s));
            }
            var MinD = DelkyCesty.Min();
            return NejkratsiCesta.First(s => DelkyCesty[NejkratsiCesta.IndexOf(s)] == MinD);
            //return SeznamS.First(s => DelkaCesty(CreateCesta(FromI, ToI, s)) == SeznamS.Min(x => DelkaCesty(CreateCesta(FromI, ToI, x))));
        }
        public int[] CreateCesta(int FromI, int ToI, string cesta)
        {
            List<int> l = new List<int>();
            l.Add(FromI);
            if (cesta != "") l.AddRange(cesta.Replace("\"", "").Split(',').Select(x => int.Parse(x)));
            l.Add(ToI);
            return l.ToArray();
        }
        public double DelkaCesty(int[] cesta)
        {
            double suma = 0;
            for (int i = 1; i < cesta.Length; i++)
            {
                suma += Delky[cesta[i - 1], cesta[i]];
            }
            return suma;
        }
        int MaxI = 40;
        public void SearchPath(List<string> NejkratsiCesta, string s, int FromI, int FromFromI, int ToI, int ind)
        {
            if (NejkratsiCesta.Count > 0 && ind > NejkratsiCesta[0].Length / 4) return;
            int inde = ind + 1;
            if (inde > MaxI) return;
            var ss = (from f in Ribs[FromI].NextWalkRib.Split(',') select int.Parse(f)).ToArray();
            if (FromI == ToI || ss.Contains(ToI))
            {
                if (NejkratsiCesta.Count > 0)
                {
                    if (s.Length < NejkratsiCesta[0].Length)
                    {
                        NejkratsiCesta.Clear();
                        NejkratsiCesta.Add(s);
                    }
                    else if (s.Length == NejkratsiCesta[0].Length) NejkratsiCesta.Add(s);
                }
                else NejkratsiCesta.Add(s);
            }
            else
            {
                foreach (var i in ss)
                {
                    if (i != FromI && i != FromFromI)
                    {
                        string S = s;
                        if (s == "") S += "\"" + i + "\"";
                        else S += ",\"" + i + "\"";
                        SearchPath(NejkratsiCesta, S, i, FromI, ToI, inde);
                    }
                }
                //if (S != "") SeznamS.Add(S);
            }
        }

        private void Click_Save(object sender, RoutedEventArgs e)
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var sw = new System.IO.StreamWriter(sfd.FileName);
                for (int i = 0; i < Ribs.Count; i++)
                {
                    sw.WriteLine(Ribs[i].ID + " " + Ribs[i].P.ToString() + " " + Ribs[i].NextWalkRib);
                }
                sw.Close();
            }
        }

        private void Click_Load(object sender, RoutedEventArgs e)
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var sr = new System.IO.StreamReader(ofd.FileName);
                var radek = sr.ReadLine();
                while(radek!=null)
                {
                    var s = radek.Split(' ');
                    if (s.Length != 3) Console.WriteLine("Line corrupted.");
                    else
                    {
                        Ribs.Add(new WalkRib() { ID = int.Parse(s[0]), P = Point.Parse(s[1].Replace(';', ',')), NextWalkRib = s[2] });
                    }
                    radek = sr.ReadLine();
                }
                sr.Close();
                listBox.Items.Clear();
                var ID = (from f in Ribs select f.ID).Max() + 1;
                for (int id = 0; id < ID; id++)
                {
                    listBox.Items.Add(id);
                }
                CreateGraphics();
                CreateRibs();
            }
        }
    }
}
