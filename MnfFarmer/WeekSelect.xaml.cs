using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interakční logika pro WeekSelect.xaml
    /// </summary>
    public partial class WeekSelect : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public WeekSelect()
        {
            InitializeComponent();
            Casy = new List<ObservableCollection<bool>>();
            Binding b = new Binding("UseOne");
            b.Mode = BindingMode.TwoWay;
            b.NotifyOnSourceUpdated = true;
            b.Source = this;
            cb.SetBinding(CheckBox.IsCheckedProperty, b);
            b = new Binding("Delay");
            b.Mode = BindingMode.TwoWay;
            b.Source = this;
            t.SetBinding(TextBox.TextProperty, b);
        }
        public List<ObservableCollection<bool>> Casy;
        private bool _UseOne = false;
        public bool UseOne { get { return _UseOne; } set { _UseOne = value; UseOneN = !value; OnPropertyChanged("UseOne"); } }
        private bool _UseOneN = false;
        public bool UseOneN { get { return _UseOneN; } set { _UseOneN = value; OnPropertyChanged("UseOneN"); } }
        private uint _Delay = 10;
        public uint Delay { get { return _Delay; } set { _Delay = value; OnPropertyChanged("Delay"); } }
        public void Init(string[] d, int h)
        {
            sp.Children.Clear();
            for (int i = 0; i < d.Length; i++)
            {
                var v = new ObservableCollection<bool>();
                for (int j = 0; j < h; j++)
                {
                    v.Add(false);
                }
                Casy.Add(v);
                if (i == 0)
                {
                    var ts =  new TimeSelect();
                    ts.Init(v, d[i]);
                    sp.Children.Add(ts);
                }
                else
                {
                    var ts = new TimeSelect2();
                    ts.Init(v, d[i]);
                    sp.Children.Add(ts);
                    Binding b = new Binding("UseOneN");
                    b.Mode = BindingMode.TwoWay;
                    b.NotifyOnSourceUpdated = true;
                    b.Source = this;
                    ts.SetBinding(UIElement.IsEnabledProperty, b);
                }
            }
        }
        
    }
}
