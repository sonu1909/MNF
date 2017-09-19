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
    /// Interaction logic for MnfLogin.xaml
    /// </summary>
    public partial class MnfLogin : Window
    {
        public MnfLogin()
        {
            InitializeComponent();
            User = new Uzivatel();
            DataContext = User;
        }
        public Uzivatel User;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
