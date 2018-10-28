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
    /// Interakční logika pro SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        public string Ucet = "";
        public string Postava = "";

        public SetupWindow()
        {
            InitializeComponent();
            
        }
    }
}
