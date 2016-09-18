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

namespace MnfPic
{
    /// <summary>
    /// Interaction logic for ShowSelectServer.xaml
    /// </summary>
    public partial class ShowSelectServer : Window
    {
        public ShowSelectServer()
        {
            InitializeComponent();
        }

        public void Init(MnfServer[] ms)
        {
            foreach (MnfServer a in ms)
            {
                if (a.Kapacita == 0) lb.Items.Add(a.Jmeno + " 0%");
                else lb.Items.Add(a.Jmeno + " " + ((a.PocetMuzu + a.PocetZen) * 100.0 / a.Kapacita).ToString("0.0") + "%");
            }
        }
        public int SlectedServer = -1;
        private void lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SlectedServer = lb.SelectedIndex;
            DialogResult = true;
            Close();
        }
    }
}
