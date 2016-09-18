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
    /// Interaction logic for ShowSelectAvatar.xaml
    /// </summary>
    public partial class ShowSelectAvatar : Window
    {
        public ShowSelectAvatar()
        {
            InitializeComponent();
        }

        public void Init(MnfAvatar[] ma)
        {
            foreach (MnfAvatar a in ma) lb.Items.Add(a.Name);
        }
        public int SlectedAvatar = -1;
        private void lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SlectedAvatar = lb.SelectedIndex;
            DialogResult = true;
            Close();
        }
    }
}
