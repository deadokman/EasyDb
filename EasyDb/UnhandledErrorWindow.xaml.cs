using System;
using System.Media;
using System.Windows;
using MahApps.Metro.Controls;

namespace EasyDb
{
    /// <summary>
    /// Interaction logic for UnhandledErrorWindow.xaml
    /// </summary>
    public partial class UnhandledErrorWindow : MetroWindow
    {
        public UnhandledErrorWindow(Exception ex, Window owner)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Owner = owner;
            this.MainErrMessage.Text = ex.Message +"\n" + ex.InnerException?.Message;
            MainExText.AppendText(ex.ToString());
            SystemSounds.Hand.Play();
            this.Focus();
        }
    }
}
