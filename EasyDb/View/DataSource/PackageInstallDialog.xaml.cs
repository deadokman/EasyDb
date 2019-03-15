using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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

        private FlowDocument _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageInstallDialog"/> class.
        /// Package install dialog
        /// </summary>
        /// <param name="controller"> Chocolattey controller</param>
        public PackageInstallDialog([NotNull] IChocolateyController controller)
        {
            this._controller = controller ?? throw new ArgumentNullException(nameof(controller));
            InitializeComponent();
            _document = new FlowDocument();
            LogsTextBox.Document = _document;
        }

        /// <summary>
        /// handle chocolatey message
        /// </summary>
        /// <param name="level">Message level</param>
        /// <param name="message">text</param>
        /// <param name="ex">Exception</param>
        public void ChocoMessage(MessageLevel level, string message, Exception ex = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SolidColorBrush foregroundBrush;
                switch (level)
                {
                    case MessageLevel.Error:
                    case MessageLevel.Fatal:
                        foregroundBrush = new SolidColorBrush(Colors.Red);
                        break;
                    case MessageLevel.Warning:
                        foregroundBrush = new SolidColorBrush(Colors.Yellow);
                        break;
                    default:
                        foregroundBrush = (SolidColorBrush)App.Current.Resources["AccentColorBrush"];
                        break;
                }
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(message)
                {
                    Foreground = foregroundBrush
                });

                _document.Blocks.Add(paragraph);
                if (ex != null)
                {
                    paragraph.Inlines.Add(new Run(message)
                    {
                        Foreground = new SolidColorBrush(Colors.Red)
                    });
                }

                LogsTextBox.ScrollToEnd();
            });
        }

        private void PackageInstallDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            this._controller.RegisterLisner(this);
            LogsTextBox.Document.Blocks.Clear();
        }

        private void PackageInstallDialog_OnUnloaded(object sender, RoutedEventArgs e)
        {
            this._controller.UnregisterLisner(this);
        }
    }
}
