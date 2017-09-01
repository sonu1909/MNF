using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MnfPicReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        List<Image> boxy = new List<Image>();
        List<Obrazek> cestySource = new List<Obrazek>();
        List<Obrazek> cesty = new List<Obrazek>();
        List<string> Owneri = new List<string>();


        public MainWindow()
        {
            NastaveniMnfPicReader.Load();
            InitializeComponent();

            if (NastaveniMnfPicReader.MainFile == "")
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    NastaveniMnfPicReader.MainFile = fbd.SelectedPath + "\\";
                }
            }

            Fill(NastaveniMnfPicReader.MainFile);
            SizeChanged += MainWindow_SizeChanged;
            Loaded += MainWindow_Loaded;
            cestySource = cestySource.OrderBy(t => t.DatumVytvoreni).ToList();
            cestySource.Reverse();
            cesty.AddRange(cestySource);


            MaximumSB = 50;
            ValueSB = 0;

            LB.Items.Add("All");
            foreach (string s in Owneri)
                LB.Items.Add(s);
            if (LB.Items.Count > 0)
                LB.SelectedIndex = 0;
            LB.SelectionChanged += LB_SelectionChanged;

            Binding b = new Binding("MaximumSB");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            scrollBar.SetBinding(ScrollBar.MaximumProperty, b);

            b = new Binding("ValueSB");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            scrollBar.SetBinding(ScrollBar.ValueProperty, b);
            
            scrollBar.ValueChanged += ScrollBar_ValueChanged;
            showGrid.MouseWheel += ShowGrid_MouseWheel;
            scrollBar.MouseWheel += ShowGrid_MouseWheel;

            textBox.TextChanged += TextBox_TextChanged;

            var v = (from f in cestySource where f.Owner == "" && (from c in cestySource where System.IO.Path.GetFileName(f.Cesta) == System.IO.Path.GetFileName(c.Cesta) select c).Count() > 1 select f).ToArray();
            Console.WriteLine("To Delete: " + v.Count());
            foreach (var vv in v)
            {
                File.Delete(vv.Cesta);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LB.Items.Clear();
            if (textBox.Text=="")
            {
                LB.Items.Add("All");
                foreach (string s in Owneri)
                    LB.Items.Add(s);
            }
            else
            {
                foreach (string s in from o in Owneri where o.Contains(textBox.Text) select o)
                    LB.Items.Add(s);
            }
            if (LB.Items.Count > 0)
                LB.SelectedIndex = 0;
        }

        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LB.SelectedIndex < 0) return;
            string s = LB.SelectedItem.ToString();
            cesty.Clear();
            if (s == "All") cesty.AddRange(cestySource);
            else
            {
                cesty.AddRange(from a in cestySource where a.Owner == s select a);
            }
            MainWindow_SizeChanged(null, null);
        }

        private void ShowGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            int pocet = (int)(showGrid.ActualHeight / NastaveniMnfPicReader.ImgSize);
            ValueSB += e.Delta > 0 ? -pocet : pocet;
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ValueSB = (int)scrollBar.Value;
            if (!IsLoaded) return;
            for (int i = 0; i < boxy.Count; i++)
            {
                Image image = boxy[i];
                if (i + ValueSB >= cesty.Count) return;

                Obrazek o = cesty[i + ValueSB];

                try
                {
                    var buffer = File.ReadAllBytes(o.Cesta);
                    MemoryStream ms = new MemoryStream(buffer);

                    BitmapImage  Mini = new BitmapImage();
                    Mini.BeginInit();
                    Mini.StreamSource = ms;
                    Mini.DecodePixelWidth = NastaveniMnfPicReader.ImgSize; //Your wanted image width 
                    Mini.DecodePixelHeight = NastaveniMnfPicReader.ImgSize; //(int)(NastaveniMnfPicReader.ImgSize * Mini.Height/Mini.Width);//Your wanted image height
                    Mini.EndInit();
                    Mini.Freeze();

                    //cesty[i + ValueSB].Init();
                    image.Source = Mini;
                }
                catch (Exception ee) { Console.WriteLine(ee); }

            }
        }
        public double MaximumSB { get { return _MaximumSB; } set { _MaximumSB = value; OnPropertyChanged("MaximumSB"); } }
        double _MaximumSB = 10;
        public int ValueSB { get { return _ValueSB; } set { _ValueSB = value; OnPropertyChanged("ValueSB"); } }
        int _ValueSB = 0;


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        //private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    boxy.Clear();
        //    sp.Children.Clear();
        //    int pocet = (int)(ActualWidth / NastaveniMnfPicReader.ImgSize);
        //    int i = 0;
        //    for (i = 0; i < pocet; i++)
        //    {
        //        ListBox lb = new ListBox() { Width = NastaveniMnfPicReader.ImgSize, };
        //        lb.MouseDoubleClick += ListBox_MouseDoubleClick;
        //        boxy.Add(lb);
        //        //ScrollViewer sv = new ScrollViewer() { Width = NastaveniMnfPicReader.ImgSize, };                
        //        //sv.Content = lb;
        //        sp.Children.Add(lb);
        //    }
        //    i = 0;
        //    foreach (Obrazek o in cesty) if (o.Mini != null) boxy[i++ % boxy.Count].Items.Add(new Image() { Source = o.Mini, Height = 200 });
        //}
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded) return;
            boxy.Clear();
            int pocetW = (int)(showGrid.ActualWidth / NastaveniMnfPicReader.ImgSize);
            int pocetH = (int)(showGrid.ActualHeight / NastaveniMnfPicReader.ImgSize);
            MaximumSB = (double)(cesty.Count - pocetH * pocetW);
            //ValueSB = 1;
            //showGrid clear
            showGrid.Children.Clear();
            showGrid.RowDefinitions.Clear();
            showGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < pocetH; i++)
            {
                showGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < pocetW; i++)
            {
                showGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < pocetW; j++)
            {
                for (int i = 0; i < pocetH; i++)
                {
                    Image image = new Image();
                    image.MouseUp += Image_MouseUp;
                    //image.Stretch = Stretch.Fill;
                    boxy.Add(image);
                    Grid.SetColumn(image, i);
                    Grid.SetRow(image, j);
                    showGrid.Children.Add(image);
                }
            }
            ScrollBar_ValueChanged(null, null);
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Window w = new Window();
            Image i = new Image();
            string cesta = cesty[boxy.IndexOf(sender as Image) + ValueSB].Cesta;
            string[] s = cesta.Split('\\');
            w.Title = s[s.Count() - 2];
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(cesta);
            b.EndInit();
            i.Source = b;
            w.Content = i;
            w.ShowDialog();
        }

        //private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (!IsLoaded) return;
        //    boxy.Clear();
        //    sp.Children.Clear();
        //    int pocet = (int)(ActualWidth / NastaveniMnfPicReader.ImgSize);
        //    StackPanel spp = new StackPanel() { Orientation = Orientation.Horizontal, };
        //    int j = 0;
        //    foreach (Obrazek o in cesty)
        //    {
        //        if (j >= 500) break;
        //        if (o.Mini != null)
        //        {
        //            Image i = new Image() { Source = o.Mini, Height = 200, };
        //            i.MouseUp += o.OnClick;
        //            spp.Children.Add(i);

        //            if (++j % pocet == 0)
        //            {
        //                //lock (locker)
        //                //{
        //                //    Thread.CurrentThread.SetApartmentState(ApartmentState.STA); 
        //                sp.Children.Add(spp);
        //                spp = new StackPanel() { Orientation = Orientation.Horizontal, };
        //                //}
        //            }
        //        }
        //    }
        //    sp.Children.Add(spp);
        //}
        //object locker = new object();
        string[] extension = new string[] { "bmp", "jpg", "jpeg", "png" };
        public void Fill(string cesta)
        {
            string owner = System.IO.Path.GetFileName(cesta);
            if (!Owneri.Contains(owner)) Owneri.Add(owner);
            Parallel.ForEach(Directory.GetFiles(cesta), s =>
            {
                if (extension.Contains(System.IO.Path.GetExtension(s).Replace(".", "")))
                    cestySource.Add(new Obrazek(s) { Owner = owner });
            });
            foreach (string s in Directory.GetDirectories(cesta)) Fill(s);
        }
    }
}
