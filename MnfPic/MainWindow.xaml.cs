using System;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace MnfPic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            NastaveniMnfPic.Load();
            InitializeComponent();
            Closed += MainWindow_Closed;
            Loaded += MainWindow_Loaded;
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            //if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    mainFile = fbd.SelectedPath + "\\";
            //}
            //else Close();
            NastaveniMnfPic.LoginPaswCrypted = NastaveniMnfPic.LoginPaswCrypted.ToLower();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PrihlasSe();
            controlUser.Init(Uzivatel);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
        }

        WebClient wc = new WebClient();
        
        MnfUser Uzivatel;
        int UserID
        {
            get { return Uzivatel.UserID; }
        }


        public bool PrihlasSe()
        {
            if (NastaveniMnfPic.LoginUser=="")
            {
                MnfLogin ml = new MnfLogin();
                if (ml.ShowDialog().Value)
                {
                    NastaveniMnfPic.LoginUser = ml.Email;
                    NastaveniMnfPic.LoginPaswCrypted = CalculateMD5Hash(ml.Email + ml.Password).ToLower();
                }
                else return true;
            }

            string s;
            //Logovani
            var data = new NameValueCollection();
            data["email"] = NastaveniMnfPic.LoginUser;
            data["pass"] = NastaveniMnfPic.LoginPaswCrypted.ToLower();
            Uzivatel = new MnfUser(NastaveniMnfPic.LoginPaswCrypted);

            var response = wc.UploadValues(MnfAddress.SiteMain + MnfAddress.SiteLogin, "POST", data);//&errors=00&user_id=1880483&premium=0&premium_notification=0&overcrowder=0&
            s = Encoding.UTF8.GetString(response, 0, response.Length);
            if (Uzivatel.StringParse(s)) { MessageBox.Show("Bad login\n" + s); return true; }
            else return false;
        }
        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
