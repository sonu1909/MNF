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
                });
            }
        }
        public void CreateRibs()
        {
            stackPanel.Children.Clear();
            stackPanelN.Children.Clear();
            for (int i = 0; i < Ribs.Count; i++)
            {
                Label l = new Label() { Content = i, };
                stackPanel.Children.Add(l);

                var tb = new TextBox();
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

        private void Click_Generate(object sender, RoutedEventArgs e)
        {
            //    //Ribs[i].Add(new WalkRib(Math.Round(Math.Sqrt(Math.Pow(Points[i].X - Points[j].X, 2) + Math.Pow(Points[i].Y - Points[j].Y, 2)), 2)));
            //    StringBuilder sb = new StringBuilder();
            //    foreach (var polygon in ListPolygon)
            //    {
            //        //walk_manager.AddPointGroup(new Array(new WalkPoint(449.2, 249.55), new WalkPoint(486.85, 249.55)));
            //        sb.Append("walk_manager.AddPointGroup(new Array(");
            //        sb.Append("new WalkPoint(" + polygon[0].X + ", " + polygon[0].Y + ")");
            //        for (int i = 1; i < polygon.Length; i++)
            //        {
            //            sb.Append(", new WalkPoint(" + polygon[i].X + ", " + polygon[i].Y + ")");
            //        }
            //        sb.AppendLine("));");
            //    }
            //    sb.Append("walk_manager.ribs = new Array(");
            //    sb.Append("new Array(");
            //    sb.Append("new WalkRib(" + Ribs[0][0].D.ToString().Replace(',', '.'));
            //    if (Ribs[0][0].Pole != "") sb.Append(",new Array(" + Ribs[0][0].Pole + ")");
            //    sb.Append(")");
            //    for (int j = 1; j < Ribs[0].Count; j++)
            //    {
            //        sb.Append(",new WalkRib(" + Ribs[0][j].D.ToString().Replace(',', '.'));
            //        if (Ribs[0][j].Pole != "") sb.Append(",new Array(" + Ribs[0][j].Pole + ")");
            //        sb.Append(")");
            //    }
            //    sb.Append(")");
            //    for (int i = 1; i < Ribs.Count; i++)
            //    {
            //        sb.Append(",new Array(");
            //        sb.Append("new WalkRib(" + Ribs[i][0].D.ToString().Replace(',', '.'));
            //        if (Ribs[i][0].Pole != "") sb.Append(",new Array(" + Ribs[i][0].Pole + ")");
            //        sb.Append(")");
            //        for (int j = 1; j < Ribs[i].Count; j++)
            //        {
            //            sb.Append(",new WalkRib(" + Ribs[i][j].D.ToString().Replace(',', '.'));
            //            if (Ribs[i][j].Pole != "") sb.Append(",new Array(" + Ribs[i][j].Pole + ")");
            //            sb.Append(")");
            //        }
            //        sb.Append(")");
            //    }
            //    sb.AppendLine(");");
            //    Console.WriteLine(sb.ToString());
        }
        List<string> SeznamS = new List<string>();
        int MaxI = 40;
        public void SearchPath(string s, int FromI, int FromFromI, int ToI, int ind)
        {
            int inde = ind + 1;
            if (inde > MaxI) return;
            var ss = (from f in Ribs[FromI].NextWalkRib.Split(',') select int.Parse(f)).ToArray();
            if (FromI == ToI) SeznamS.Add(s);
            else if (ss.Contains(ToI)) SeznamS.Add(s);
            else
            {
                string S = s;
                foreach (var i in ss)
                {
                    if (i != FromI && i != FromFromI)
                    {
                        if (s == "") S += "\"" + FromI + "\"";
                        else S += ",\"" + FromI + "\"";
                        SearchPath(S, i, FromI, ToI, inde);
                    }
                }
                if (S != "") SeznamS.Add(S);
            }
        }
    }
}
