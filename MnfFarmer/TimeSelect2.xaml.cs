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

namespace MnfFarmer
{
    /// <summary>
    /// Interakční logika pro TimeSelect2.xaml
    /// </summary>
    public partial class TimeSelect2 : UserControl
    {
        public TimeSelect2()
        {
            InitializeComponent();
        }
        public string Popisek { get; set; }
        public void Init(ObservableCollection<bool> data, string popisek)
        {
            Popisek = popisek;
            Binding b = new Binding("Popisek");
            b.Mode = BindingMode.OneWay;
            b.Source = this;
            l.SetBinding(Label.ContentProperty, b);
            //DataContext = data;
            for (int i = 0; i < data.Count; i++)
            {
                g.ColumnDefinitions.Add(new ColumnDefinition());
                var cbb = new CheckBox() { Content = null, Height = 15, Width = 15, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Top, VerticalAlignment = VerticalAlignment.Top };
                b = new Binding("[" + i + "]");
                b.Mode = BindingMode.TwoWay;
                b.Source = data;
                cbb.SetBinding(CheckBox.IsCheckedProperty, b);
                var cb = new Viewbox();
                cb.Child = cbb;                
                Grid.SetColumn(cb, i + 1);
                g.Children.Add(cb);
            }
        }
    }
}
