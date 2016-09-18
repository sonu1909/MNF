using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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

namespace JustDownload
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebClient wc = new WebClient();

        public MainWindow()
        {
            InitializeComponent();

            if ( == "")
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mainFile = fbd.SelectedPath + "\\";
                }
                else Close();
            }

            wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
        }

        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("hotovo");
        }
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
            string[] ss = textBox.Text.Split('\'');
            List<string> obrazky = new List<string>();
            for (int i = 0; i < ss.Length; i++)
            {
                if (i % 2 == 1) obrazky.Add(ss[i]);
            }
            foreach (string o in obrazky)
            {
                string oo = mainFile + o.Split('/')[5];
                if (!File.Exists(oo)) wc.DownloadFile(new Uri(o.Replace(".jpg", "_.jpg")), oo);
            }

        }
    }
}
