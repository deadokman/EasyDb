using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;

namespace EasyDb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            StoreLayout();
        }

        private void StoreLayout()
        {
            LayoutManager.StoreLayout(DockingManager);
        }

        private void DockingManager_OnLoaded(object sender, RoutedEventArgs e)
        {
            LayoutManager.LoadLayout(DockingManager);
            ////Загрузить лейаут если есть
            //if (File.Exists(@".\AvalonDock.Layout.config"))
            //{
            //    var layoutSerializer = new XmlLayoutSerializer(this.DockingManager);
            //    using (var stream = new StreamReader(@".\AvalonDock.Layout.config"))
            //    {
            //        layoutSerializer.Deserialize(stream);
            //    }
            //}
        }
    }
}
