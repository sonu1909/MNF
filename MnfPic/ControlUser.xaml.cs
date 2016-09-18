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
    /// Interaction logic for ControlUser.xaml
    /// </summary>
    public partial class ControlUser : UserControl
    {
        public ControlUser()
        {
            InitializeComponent();
            listBox.SelectionChanged += ListBox_SelectionChanged;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (User == null) return;
            if (listBox.SelectedIndex < 0) return;
            controlAvatar.Init(User.Avatary[listBox.SelectedIndex]);
        }

        MnfUser User;
        public void Init(MnfUser user)
        {
            user.NactiAvatary();

            listBox.Items.Clear();
            foreach(MnfAvatar ma in user.Avatary )
            {
                listBox.Items.Add(ma.Name);
            }
            User = user;
            DataContext = user;
        }
    }
}
