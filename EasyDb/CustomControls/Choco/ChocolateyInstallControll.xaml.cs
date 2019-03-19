namespace EasyDb.CustomControls.Choco
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ChocolateyInstallControll.xaml
    /// </summary>
    public partial class ChocolateyInstallControll : UserControl
    {
        private readonly Action _invokeDlgClose;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyInstallControll"/> class.
        /// Control that displays chocolatey installation question
        /// </summary>
        /// <param name="invokeDlgClose">Close dialog action</param>
        public ChocolateyInstallControll(Action invokeDlgClose)
        {
            this._invokeDlgClose = invokeDlgClose ?? throw new ArgumentNullException(nameof(invokeDlgClose));
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this._invokeDlgClose.Invoke();
        }
    }
}
