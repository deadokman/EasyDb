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

namespace EasyDb.View.DataSource
{
    using EasyDb.Annotations;

    using Edb.Environment.Interface;

    using NuGet;

    /// <summary>
    /// Interaction logic for PackageInstallDialog.xaml
    /// </summary>
    public partial class PackageInstallDialog : UserControl, IChocoMessageListner
    {
        private readonly IChocolateyController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageInstallDialog"/> class.
        /// Package install dialog
        /// </summary>
        /// <param name="controller"> Chocolattey controller</param>
        public PackageInstallDialog([NotNull] IChocolateyController controller)
        {
            this._controller = controller ?? throw new ArgumentNullException(nameof(controller));
            InitializeComponent();
        }

        /// <summary>
        /// handle chocolatey message
        /// </summary>
        /// <param name="level">Message level</param>
        /// <param name="message">text</param>
        /// <param name="ex">Exception</param>
        public void ChocoMessage(MessageLevel level, string message, Exception ex = null)
        {
            throw new NotImplementedException();
        }
    }
}
