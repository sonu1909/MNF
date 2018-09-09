using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace MnfFarmer
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        List<string> Dny;
        int hodin = 24;
        private bool _IsStarted = false;
        public bool IsStarted { get { return _IsStarted; } set { _IsStarted = value; OnPropertyChanged("IsStarted"); } }
        private bool _IsInGame = false;
        public bool IsInGame { get { return _IsInGame; } set { _IsInGame = value; OnPropertyChanged("IsInGame"); } }
        private string _LoggingString = "";
        public string LoggingString { get { return _LoggingString; } set { _LoggingString = value; OnPropertyChanged("LoggingString"); } }
        Timer t;
        Random r = new Random();
        MnfPic.MnfGame Game;
        MnfPic.MnfLogger Logger;
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            IsStarted = false;
            Dny = new List<string>();
            Dny.AddRange(Enum.GetNames(typeof(DayOfWeek)));
            Dny.Add(Dny.First());
            Dny.RemoveAt(0);
            ws.Init(Dny.ToArray(), hodin);

            Binding b = new Binding("LoggingString");
            b.Mode = BindingMode.OneWay;
            b.Source = this;
            lSetup.SetBinding(Label.ContentProperty, b);

            t = new Timer(tick);
            LoggingString = Properties.Settings.Default.LogingString;
            var s = Properties.Settings.Default.DayInit.ToCharArray();
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    ws.Casy[i / hodin][i % hodin] = s[i] == '1';
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            ws.UseOne = Properties.Settings.Default.AllDaySame;
            ws.Delay = Properties.Settings.Default.Delay;
            Game = new MnfPic.MnfGame();
            Logger = new MnfPic.MnfLogger();
        }

        private void tick(object state)
        {
            t.Change(Timeout.Infinite, Timeout.Infinite);
            if (IsStarted)
            {
                Thread.Sleep(r.Next(0, ws.Delay > 55 ? 55 : (int)ws.Delay) * 60000 + r.Next(100, 60000));
                if (IsTime())
                {
                    //open
                    if (!IsInGame)
                    {
                        try
                        {
                            Logger.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                try
                                {
                                    Game.Close();
                                    var s = LoggingString.Split(';');
                                    Logger.LB_Select(Logger.Uzivatele.IndexOf((from f in Logger.Uzivatele where f.JmenoUzivatele == s[0] select f).First()));
                                    Thread.Sleep(500 + r.Next(1000));
                                    Logger.LBA_Select(Logger.Avatars.IndexOf((from f in Logger.Avatars where f.JmenoPostavy == s[2] select f).First()));
                                    Thread.Sleep(500 + r.Next(1000));
                                    var mp = Logger.LBS_Select(Logger.Servers.IndexOf((from f in Logger.Servers where f.JmenoServeru == s[1] select f).First()));
                                    Game.Init(mp);
                                    IsInGame = true;
                                    Game.GameID = (int)Enum.Parse(typeof(EGames), s[3]);
                                    //SpinWait.SpinUntil(() => Game.ActualArea != null);
                                    Thread.Sleep(8000 + r.Next(2000));
                                    Game.BeachGameClick(null, null);
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Login Error " + e.Message);

                                    Logger.Close();
                                    Game.Close();
                                    IsInGame = false;
                                }
                            }));
                        }
                        catch(Exception e)
                        {
                            MessageBox.Show("Login Error " + e.Message);

                            Logger.Close();
                            Game.Close();
                            IsInGame = false;
                        }
                    }
                }
                else
                {
                    if (IsInGame)
                    {
                        //close
                        Logger.Close();
                        Game.Close();
                        IsInGame = false;
                    }
                }
                t.Change(60000, Timeout.Infinite);
            }
            else
            {
                if (IsInGame)
                {
                    //close
                    Logger.Close();
                    Game.Close();
                    IsInGame = false;
                }
            }
        }
        public bool IsTime()
        {
            return ws.UseOne ? ws.Casy[0][DateTime.Now.Hour] : ws.Casy[Dny.IndexOf(DateTime.Now.DayOfWeek.ToString())][DateTime.Now.Hour];
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            t?.Change(Timeout.Infinite, Timeout.Infinite);
            List<char> c = new List<char>();
            foreach (var i in ws.Casy)
            {
                foreach (var j in i)
                {
                    c.Add(j ? '1' : '0');
                }
            }
            Properties.Settings.Default.DayInit = new string(c.ToArray());
            Properties.Settings.Default.LogingString = LoggingString;
            Properties.Settings.Default.AllDaySame = ws.UseOne;
            Properties.Settings.Default.Delay = ws.Delay;
            Properties.Settings.Default.Save();

            Logger.Close();
            Game.Close();
            IsStarted = false;
        }

        private void Setup_Click(object sender, RoutedEventArgs e)
        {
            var w = new SetupWindow();
            try
            {
                var s = LoggingString.Split(';');
                w.Ucet = s[0];
                w.Server = s[1];
                w.Postava = s[2];
                w.SetGame(s[3]);
            }
            catch { }
            w.ShowDialog();
            LoggingString = w.Ucet + ";" + w.Server + ";" + w.Postava + ";" + w.Hra + ";" + w.HraPar;
            Logger = new MnfPic.MnfLogger();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            IsStarted = !IsStarted;
            if (IsStarted)
            {
                ws.IsEnabled = false;
                bSetup.IsEnabled = false;
                lSetup.IsEnabled = false;
                bStart.Content = "Stop";
                ell.Fill = Brushes.Green;
                t.Change(0, Timeout.Infinite);
            }
            else
            {
                ws.IsEnabled = true;
                bSetup.IsEnabled = true;
                lSetup.IsEnabled = true;
                bStart.Content = "Start";
                ell.Fill = Brushes.Red;
            }
        }
    }
}
