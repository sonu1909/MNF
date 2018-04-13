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
        public List<Point[]> ListPolygon = new List<Point[]>();
        public List<Point> BodyPolygonu = new List<Point>();
        public List<List<WalkRib>> Ribs = new List<List<WalkRib>>();
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
                var l = listBox.SelectedIndex;
                ListPolygon.RemoveAt(l);
                mainGrid.Children.RemoveAt(l + 1);
                listBox.Items.RemoveAt(l);
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
            ListPolygon.Add(BodyPolygonu.ToArray());
            //ListPolygon.Add(new Polygon()
            //{
            //    Points = new PointCollection(BodyPolygonu),
            //    Fill = Brushes.Blue,
            //});
            listBox.Items.Add(ListPolygon.Count);
            BodyPolygonu.Clear();
            CreateGraphics();
            CreateRibs();
        }
        public void CreateGraphics()
        {
            mainGrid.Children.RemoveRange(1, mainGrid.Children.Count - 1);
            foreach (var i in ListPolygon)
            {
                if (i.Count() > 2)
                    mainGrid.Children.Add(new Polygon()
                    {
                        Points = new PointCollection((from f in i select new Point(f.X+MainPoint.X,f.Y+MainPoint.Y)).ToArray()),
                        Fill = Brushes.Blue,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                    });
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
                else
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
            List<Point> Points = new List<Point>();
            foreach (var i in ListPolygon) Points.AddRange(i);
            int pozice = 0;
            foreach (var i in Points)
            {
                mainGrid.Children.Add(new Label()
                {
                    Content = pozice++,
                    Margin = new Thickness(i.X + MainPoint.X, i.Y + MainPoint.Y, 0, 0),
                    Foreground = Brushes.Red,
                });
            }
        }
        public void CreateRibs()
        {
            List<Point> Points = new List<Point>();
            foreach (var i in ListPolygon) Points.AddRange(i);
            var n = Points.Count;
            Ribs.Clear();
            listBox2.Items.Clear();
            for (int i = 0; i < n; i++)
            {
                Ribs.Add(new List<WalkRib>());
                for (int j = 0; j < n; j++)
                {
                    Ribs[i].Add(new WalkRib(Math.Round(Math.Sqrt(Math.Pow(Points[i].X - Points[j].X, 2) + Math.Pow(Points[i].Y - Points[j].Y, 2)), 2)));
                }
                listBox2.Items.Add(i);
            }
        }

        private void listBox_SelectionChanged2(object sender, SelectionChangedEventArgs e)
        {
            if (listBox2.SelectedIndex < 0) return;
            stackPanel.Children.Clear();
            int pocet = 0;
            foreach (var i in Ribs[listBox2.SelectedIndex])
            {
                Grid g = new Grid();
                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25), });
                g.ColumnDefinitions.Add(new ColumnDefinition());
                Label l = new Label() { Content = pocet++, };
                g.Children.Add(l);
                var tb = new TextBox();
                Binding b = new Binding("Pole");
                b.Source = i;
                b.Mode = BindingMode.TwoWay;
                tb.SetBinding(TextBox.TextProperty, b);
                Grid.SetColumn(tb, 1);
                g.Children.Add(tb);
                stackPanel.Children.Add(g);
            }
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBox.SelectedIndex < 0) return;
            StackPanel sp1 = new StackPanel();
            StackPanel sp2 = new StackPanel();
            var p = ListPolygon[listBox.SelectedIndex];
            foreach (var i in p)
            {
                sp1.Children.Add(new TextBox() { Text = i.X.ToString(), TextAlignment = TextAlignment.Center });
                sp2.Children.Add(new TextBox() { Text = i.Y.ToString(), TextAlignment = TextAlignment.Center });
            }
            var g = new Grid();
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.Children.Add(sp1);
            g.Children.Add(sp2);
            Grid.SetColumn(sp2, 1);
            var w = new Window() { Content = g };
            w.ShowDialog();
            for (int i = 0; i < p.Length; i++)
            {
                try
                {
                    ListPolygon[listBox.SelectedIndex][i].X = double.Parse((sp1.Children[i] as TextBox).Text);
                    ListPolygon[listBox.SelectedIndex][i].Y = double.Parse((sp2.Children[i] as TextBox).Text);
                }
                catch { }
            }

            CreateGraphics();
            List<Point> Points = new List<Point>();
            var n = Points.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Ribs[i][j].D = Math.Round(Math.Sqrt(Math.Pow(Points[i].X - Points[j].X, 2) + Math.Pow(Points[i].Y - Points[j].Y, 2)), 2);
                }
            }
        }

        private void Click_Generate(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var polygon in ListPolygon)
            {
                //walk_manager.AddPointGroup(new Array(new WalkPoint(449.2, 249.55), new WalkPoint(486.85, 249.55)));
                sb.Append("walk_manager.AddPointGroup(new Array(");
                sb.Append("new WalkPoint(" + polygon[0].X + ", " + polygon[0].Y + ")");
                for (int i = 1; i < polygon.Length; i++)
                {
                    sb.Append(", new WalkPoint(" + polygon[i].X + ", " + polygon[i].Y + ")");
                }
                sb.AppendLine("));");
            }
            sb.Append("walk_manager.ribs = new Array(");
            sb.Append("new Array(");
            sb.Append("new WalkRib(" + Ribs[0][0].D);
            if (Ribs[0][0].Pole != "") sb.Append(",new Array(" + Ribs[0][0].Pole + ")");
            sb.Append(")");
            for (int j = 1; j < Ribs[0].Count; j++)
            {
                sb.Append(",new WalkRib(" + Ribs[0][j].D);
                if (Ribs[0][j].Pole != "") sb.Append(",new Array(" + Ribs[0][j].Pole + ")");
                sb.Append(")");
            }
            sb.Append(")");
            for (int i = 1; i < Ribs.Count; i++)
            {
                sb.Append(",new Array(");
                sb.Append("new WalkRib(" + Ribs[i][0].D);
                if (Ribs[i][0].Pole != "") sb.Append(",new Array(" + Ribs[i][0].Pole + ")");
                sb.Append(")");
                for (int j = 1; j < Ribs[i].Count; j++)
                {
                    sb.Append(",new WalkRib(" + Ribs[i][j].D);
                    if (Ribs[i][j].Pole != "") sb.Append(",new Array(" + Ribs[i][j].Pole + ")");
                    sb.Append(")");
                }
                sb.Append(")");
            }
            sb.AppendLine(");");
            Console.WriteLine(sb.ToString());
        }
    }
}
