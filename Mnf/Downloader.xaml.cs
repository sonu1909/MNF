using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace Mnf
{
    /// <summary>
    /// Interakční logika pro Downloader.xaml
    /// </summary>
    public partial class Downloader : UserControl
    {
        public Downloader()
        {
            InitializeComponent();

            PictureBW.WorkerSupportsCancellation = true;
            PictureBW.DoWork += PictureBW_DoWork;
            //PictureBW.RunWorkerCompleted += GameBW_RunWorkerCompleted;
        }
        MnfGame MG;
        public void Init(MnfGame mg, WebClient wc)
        {
            MG = mg;
            WC = wc;
        }

        WebClient WC;
        private void getUsers(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //TODO: read old file and start from the end
                MG.AvatarsDownloadTo = new StringBuilder();
                string s = sfd.FileName;
                MG.AvatarsDownload = true;
                try
                {
                    for (int i = 10000; i < Properties.Settings.Default.AvatarMaxID; i++)
                    {
                        MG.GetAvatarDetails(i);
                        Thread.Sleep(30);//TODO:nastavitelne spozdeni
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    MG.AvatarsDownload = false;
                    File.WriteAllText(s, MG.AvatarsDownloadTo.ToString());
                }
            }
        }

        private void getBackGroundsA(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //TODO: read old file and start from the end
                string s = fbd.SelectedPath;
                for (int i = 0; i < Properties.Settings.Default.AvatarMaxID; i++)
                {
                    try
                    {
                        var n = "64" + i.ToString("00000000");
                        WC.DownloadFileAsync(new Uri(MnfAddress.SiteBG(n)), s + "//" + n + ".jpg");
                        Console.WriteLine("Downloaded " + i);
                    }
                    catch { }
                }
            }
        }

        private void getBackGroundsS(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //TODO: read old file and start from the end
                string s = fbd.SelectedPath;
                for (int i = 0; i < Properties.Settings.Default.AvatarMaxID; i++)
                {
                    try
                    {
                        var n = "66" + i.ToString("00000000");
                        WC.DownloadFileAsync(new Uri(MnfAddress.SiteBG(n)), s + "//" + n + ".jpg");
                        Console.WriteLine("Downloaded " + i);
                    }
                    catch { }
                }
            }
        }
        private void getBackGroundsJ(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            //if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    //TODO: read old file and start from the end
            //    string s = fbd.SelectedPath;
            //    for (int i = 0; i < AvatarMaxID; i++)
            //    {
            //        try
            //        {
            //            var n = "66" + i.ToString("00000000");
            //            wc.DownloadFileAsync(new Uri(MnfAddress.SiteBG(n)), s + "//" + n + ".jpg");
            //            Console.WriteLine("Downloaded " + i);
            //        }
            //        catch { }
            //    }
            //}
        }

        BackgroundWorker PictureBW = new BackgroundWorker();

        public void GetAllPicture()
        {
            getPicture(null, null);
        }

        private void getPicture(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.SaveImages) if (!PictureBW.IsBusy) PictureBW.RunWorkerAsync();
        }
        private void PictureBW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Properties.Settings.Default.SaveImages)
            {
                MG.SavedPictures = 0;
                MG.GoToArea(MnfArea.Lokace[19]);
                if (PictureBW.CancellationPending) return;
                MG.GoToArea(MnfArea.Lokace[20]);
                if (PictureBW.CancellationPending) return;
                MG.SavePicture = true;
                MG.GoToArea(MnfArea.Lokace[21]);
                if (PictureBW.CancellationPending) return;
                SpinWait.SpinUntil(() => MG.SavePicture == false || PictureBW.CancellationPending, 20000);
                if (MG.SavePicture == true) return;
                MG.SavePicture = true;
                MG.GoToArea(MnfArea.Lokace[22]);
                if (PictureBW.CancellationPending) return;
                SpinWait.SpinUntil(() => MG.SavePicture == false || PictureBW.CancellationPending, 20000);
                if (MG.SavePicture == true) return;
                MG.SavePicture = true;
                MG.GoToArea(MnfArea.Lokace[23]);
                if (PictureBW.CancellationPending) return;
                SpinWait.SpinUntil(() => MG.SavePicture == false || PictureBW.CancellationPending, 20000);
                if (MG.SavePicture == true) return;
                MG.SavePicture = true;
                MG.GoToArea(MnfArea.Lokace[24]);
                if (PictureBW.CancellationPending) return;
                SpinWait.SpinUntil(() => MG.SavePicture == false || PictureBW.CancellationPending, 20000);
                if (MG.SavePicture == true) return;
            }
        }
        public void Close()
        {
            PictureBW.CancelAsync();
        }

        private void getList(object sender, RoutedEventArgs e)
        {
            MG.GetFriendList();
        }
    }
}
