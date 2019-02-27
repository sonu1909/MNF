using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Mnf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window { 


        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
            Loaded += MainWindow_Loaded;
            mnfLogger.NewUser += MnfLogger_NewUser;
        }

        private void MnfLogger_NewUser(object sender, MnfPlayer e)
        {
            mnfGame.Close();
            mnfGame.Init(e);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }


        private void MainWindow_Closed(object sender, EventArgs e)
        {
            mnfLogger.Close();
            mnfGame.Close();
        }

    }
}
