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

namespace MnfPic
{
    /// <summary>
    /// Interaction logic for MnfAvatarInfo.xaml
    /// </summary>
    public partial class MnfAvatarInfo : UserControl
    {
        public MnfAvatarInfo()
        {
            InitializeComponent();
        }

        MnfGame MG = null;
        MnfAvatar MA = null;

        public void Init(MnfGame mg)
        {
            MG = mg;
        }
        public void Init(MnfAvatar ma)
        {
            DataContext = ma;
            MA = ma;
        }

        private void Click_Invite(object sender, RoutedEventArgs e)
        {
            if (MA == null) return;

        }

        private void Click_To_Chat(object sender, RoutedEventArgs e)
        {
            if (MA == null) return;
            MG?.ChatPostavy.Add(MA);

        }

        private void Click_From_Friends(object sender, RoutedEventArgs e)
        {
            if (MA == null) return;
            MG?.RemoveFriend(MA.AvatarID);
        }

        private void Click_To_Friends(object sender, RoutedEventArgs e)
        {
            if (MA == null) return;
        }

        private void Click_From_Ignor(object sender, RoutedEventArgs e)
        {
            if (MA == null) return;
            MG?.RemIgnor(MA.AvatarID);
        }

        private void Click_To_Ignor(object sender, RoutedEventArgs e)
        {
            if (MA == null) return;
            MG?.SetIgnor(MA.AvatarID);
        }

        private void Click_From_Jail(object sender, RoutedEventArgs e)
        {
            if (MA == null) return;
            MG?.FromJail(MA.AvatarID);
        }

        private void Click_To_Jail(object sender, RoutedEventArgs e)
        {
            if (MA == null) return;
            MG?.ToJail(MA.AvatarID);
        }
    }
}
