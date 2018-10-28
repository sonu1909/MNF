using System;
using System.Collections.Generic;
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

namespace Mnf
{
    /// <summary>
    /// Interaction logic for ControlTranslator.xaml
    /// </summary>
    public partial class ControlTranslator : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public ControlTranslator()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// The URL used to speak the translation.
        /// </summary>
        private string _translationSpeakUrl;

        private string _ItemJazykZ;
        public string ItemJazykZ
        {
            get { return _ItemJazykZ; }
            set { _ItemJazykZ = value; OnPropertyChanged("ItemJazykZ"); }
        }

        private string _ItemJazykDo;
        public string ItemJazykDo
        {
            get { return _ItemJazykDo; }
            set { _ItemJazykDo = value; OnPropertyChanged("ItemJazykDo"); }
        }

        private string _ZpravaZ;
        public string ZpravaZ
        {
            get { return _ZpravaZ; }
            set { _ZpravaZ = value; OnPropertyChanged("ZpravaZ"); }
        }

        private string _ZpravaDo;
        public string ZpravaDo
        {
            get { return _ZpravaDo; }
            set { _ZpravaDo = value; OnPropertyChanged("ZpravaDo"); }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var v in Translator.Languages.ToArray())
            {
                JazykZ.Items.Add(v);
                JazykDo.Items.Add(v);
            }
            ItemJazykZ = "English";
            ItemJazykDo = "Czech";
        }

        private void translate(object sender, RoutedEventArgs e)
        {
            // Initialize the translator
            Translator t = new Translator();
            
            _translationSpeakUrl = null;

            // Translate the text
            try
            {
                ZpravaDo = t.Translate(ZpravaZ.Trim(), ItemJazykZ, ItemJazykDo);
                if (t.Error == null)
                {
                    this._translationSpeakUrl = t.TranslationSpeechUrl;
                }
                else
                {
                    throw new Exception("Translate error");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine(string.Format("Translated in {0} mSec", (int)t.TranslationTime.TotalMilliseconds));
            }
        }

        private void translateR(object sender, RoutedEventArgs e)
        {
            // Initialize the translator
            Translator t = new Translator();

            _translationSpeakUrl = null;

            // Translate the text
            try
            {
                ZpravaZ = t.Translate(ZpravaDo.Trim(), ItemJazykDo, ItemJazykZ);
                if (t.Error == null)
                {
                    this._translationSpeakUrl = t.TranslationSpeechUrl;
                }
                else
                {
                    throw new Exception("Translate error");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine(string.Format("Translated in {0} mSec", (int)t.TranslationTime.TotalMilliseconds));
            }
        }
    }
}
