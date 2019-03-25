namespace EasyDb
{
    using System.ComponentModel;
    using System.Windows;

    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Closing += OnClosing;
        }

        /// <summary>
        /// The DockingManager_OnLoaded
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        private void DockingManager_OnLoaded(object sender, RoutedEventArgs e)
        {
            LayoutManager.LoadLayout(DockingManager);
        }

        /// <summary>
        /// The OnClosing
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="cancelEventArgs">The cancelEventArgs<see cref="CancelEventArgs"/></param>
        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            StoreLayout();
        }

        /// <summary>
        /// The StoreLayout
        /// </summary>
        private void StoreLayout()
        {
            LayoutManager.StoreLayout(DockingManager);
        }
    }
}