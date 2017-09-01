using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MnfPic
{
    /// <summary>
    /// Interaction logic for ControlServer.xaml
    /// </summary>
    public partial class ControlServer : UserControl
    {
        public ControlServer()
        {
            InitializeComponent(); 
            foreach (string s in ma.Jmena)
            {
                comboBox.Items.Add(s);
            }
        }

        MnfArea ma = new MnfArea();
        MnfServer Ms;

        public void Init(MnfServer ms)
        {
            ms.AktivujAvatar();
            ms.VyberServery();

            DataContext = ms;
            Ms = ms;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (Ms == null) return;
            Ms.GoToArea(comboBox.SelectedIndex);

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DownloadAll();
        }
        public void DownloadAll()
        {
            Ms.GoToArea(25);
            Thread.Sleep(200);
            Ms.GoToArea(26);
            Thread.Sleep(200);
            Ms.GoToArea(27);
            Thread.Sleep(200);
            Ms.GoToArea(28);
            Thread.Sleep(200);
            Ms.GoToArea(29);
            Thread.Sleep(200);
            Ms.GoToArea(30);
            Thread.Sleep(200);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            Ms.GoToArea(4);
            while (true)
            {
                Ms.Game(2);
                Thread.Sleep(5000);
            }
        }
    }
}
