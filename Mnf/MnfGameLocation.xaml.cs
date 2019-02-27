using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Mnf
{
    /// <summary>
    /// Interakční logika pro MnfGameLocation.xaml
    /// </summary>
    public partial class MnfGameLocation : UserControl
    {
        public MnfGameLocation()
        {
            InitializeComponent();

            foreach (var v in MnfArea.Lokace)
            {
                comboBoxArea.Items.Add(v.JmenoLokace);
            }
        }
        MnfGame MG;
        public void Init(MnfGame mg)
        {
            MG = mg;
            DataContext = mg.ActualArea;
            LBchat.ItemsSource = MG.ChatMsg;
        }
        private void SendMsg(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TBchat.Text)) MG.SendMsg(TBchat.Text);
        }               
        private void comboBoxArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxArea.SelectedIndex < 0) return;
            MG.GoToArea(MnfArea.Lokace[comboBoxArea.SelectedIndex]);
        }
        private void LbAktualniPostavy_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbAktualniPostavy.SelectedIndex < 0) return;
            MnfAvatar ma = MG.ActualArea.AktualniPostavy[lbAktualniPostavy.SelectedIndex];
            MG.GetAvatarFullDetails(ma.AvatarID);//see full details in new window
            lbAktualniPostavy.SelectedIndex = -1;
        }
    }
}
