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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyDb.View.Choco
{
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
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this._invokeDlgClose.Invoke();
        }
    }
}
