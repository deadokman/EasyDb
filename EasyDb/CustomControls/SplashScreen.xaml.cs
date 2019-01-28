namespace EasyDb.CustomControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : UserControl
    {
        /// <summary>
        /// Defines the CurrentTextProperty
        /// </summary>
        public static readonly DependencyProperty CurrentTextProperty = DependencyProperty.Register(
            "CurrentText",
            typeof(string),
            typeof(SplashScreen),
            new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(CurrentNumberChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen"/> class.
        /// </summary>
        public SplashScreen()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the CurrentText
        /// </summary>
        public string CurrentText
        {
            get
            {
                return (string)this.GetValue(CurrentTextProperty);
            }

            set
            {
                this.SetValue(CurrentTextProperty, value);
            }
        }

        /// <summary>
        /// The CurrentNumberChanged
        /// </summary>
        /// <param name="depObj">The depObj<see cref="DependencyObject"/></param>
        /// <param name="args">The args<see cref="DependencyPropertyChangedEventArgs"/></param>
        private static void CurrentNumberChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            SplashScreen s = (SplashScreen)depObj;
            var theLabel = s.LoadTextTb;
            if (args.NewValue != null)
            {
                theLabel.Text = args.NewValue.ToString();
            }
        }
    }
}