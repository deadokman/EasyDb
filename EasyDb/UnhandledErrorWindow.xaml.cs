namespace EasyDb
{
    using System;
    using System.Media;
    using System.Windows;

    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for UnhandledErrorWindow.xaml
    /// </summary>
    public partial class UnhandledErrorWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledErrorWindow"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/></param>
        /// <param name="owner">The owner<see cref="Window"/></param>
        public UnhandledErrorWindow(Exception ex, Window owner)
        {
            this.InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Owner = owner;
            this.MainErrMessage.Text = ex.Message + "\n" + ex.InnerException?.Message;
            this.MainExText.AppendText(ex.ToString());
            SystemSounds.Hand.Play();
            this.Focus();
        }
    }
}