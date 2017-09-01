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
    /// Interaction logic for ControlAvatar.xaml
    /// </summary>
    public partial class ControlAvatar : UserControl
    {
        public ControlAvatar()
        {
            InitializeComponent();
            listBox.SelectionChanged += ListBox_SelectionChanged;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Avatar == null) return;
            if (listBox.SelectedIndex < 0) return;
            controlServer.Init(Avatar.Servery[listBox.SelectedIndex]);
        }

        MnfAvatar Avatar;
        public void Init(MnfAvatar avatar)
        {
            avatar.VyberAvatara();
            avatar.NactiServery();

            listBox.Items.Clear();
            foreach (MnfServer ms in avatar.Servery)
            {
                listBox.Items.Add(ms.Jmeno);
            }
            Avatar = avatar;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            foreach (MnfServer ms in Avatar.Servery)
            {
                try
                {
                    Console.WriteLine("Try server " + ms.Jmeno);
                    controlServer.Init(ms);
                    controlServer.DownloadAll();
                }
                catch
                {

                }
            }
        }
    }
}
