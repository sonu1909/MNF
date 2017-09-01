using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MnfPicReader
{
    public class Obrazek
    {
        public string Cesta;
        public string Owner;
        public DateTime DatumVytvoreni;
        public BitmapImage Mini;
        bool Initied = false;

        public Obrazek(string cesta)
        {
            //if (!File.Exists(cesta) || !cesta.Contains(".jpg")) return;
            if (!File.Exists(cesta)) return;
            Cesta = cesta;
            DatumVytvoreni = File.GetCreationTime(cesta);
        }

        public bool Init()
        {
            if (Initied) return true;
            try
            {
                var buffer = File.ReadAllBytes(Cesta);
                MemoryStream ms = new MemoryStream(buffer);

                Mini = new BitmapImage();
                Mini.BeginInit();
                Mini.StreamSource = ms;
                Mini.DecodePixelWidth = NastaveniMnfPicReader.ImgSize; //Your wanted image width 
                Mini.DecodePixelHeight = NastaveniMnfPicReader.ImgSize; //(int)(NastaveniMnfPicReader.ImgSize * Mini.Height/Mini.Width);//Your wanted image height
                Mini.EndInit();
                Mini.Freeze();
                Initied = true;
            }
            catch (Exception e) { Console.WriteLine(e); Mini = null; return false; }

            return true;
        }
        public void OnClick(object sender, MouseButtonEventArgs e)
        {
            Window w = new Window();
            Image ii = new Image();
            string[] s = Cesta.Split('\\');
            w.Title = s[s.Count() - 2];
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(Cesta);
            b.EndInit();
            ii.Source = b;
            w.Content = ii;
            w.ShowDialog();
        }
    }
}
