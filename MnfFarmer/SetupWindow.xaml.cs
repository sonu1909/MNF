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
using System.Windows.Shapes;

namespace MnfFarmer
{
    /// <summary>
    /// Interakční logika pro SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        public SetupWindow()
        {
            InitializeComponent();
            l.NewUser += L_NewUser;
            Closing += SetupWindow_Closing;

            foreach (var i in Enum.GetNames(typeof(EGames)))
            {
                cb.Items.Add(i);
            }
            cb.SelectedIndex = 0;
        }
        public void SetGame(string i)
        {
            cb.SelectedItem = i;
        }

        private void SetupWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            l.Close();
        }

        private void L_NewUser(object sender, MnfPic.MnfPlayer e)
        {
            Ucet = l.Uzivatele[l.UzivateleSelected].JmenoUzivatele;
            Server = l.Servers[l.ServersSelected].JmenoServeru;
            Postava = l.Avatars[l.AvatarsSelected].JmenoPostavy;
        }

        public string Ucet = "";
        public string Server = "";
        public string Postava = "";
        public string Hra = "";
        public string HraPar = "";

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Hra = cb.SelectedItem.ToString();
        }
    }
}
