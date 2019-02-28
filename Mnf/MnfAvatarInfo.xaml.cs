﻿using System;
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

namespace Mnf
{
    /// <summary>
    /// Interaction logic for MnfMyAvatarInfo.xaml
    /// </summary>
    public partial class MnfMyAvatarInfo : UserControl
    {
        public MnfMyAvatarInfo()
        {
            InitializeComponent();
        }

        MnfGame MG = null;
        MnfAvatar MA = null;

        public void Init(MnfGame mg)
        {
            MG = mg;
        }
        public void Init(MnfAvatar ma)
        {
            DataContext = ma;
            MA = ma;
        }
     }
}
