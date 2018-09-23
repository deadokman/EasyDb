using System.Windows;
using System.Windows.Controls;

namespace EasyDb.CustomControls
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : UserControl
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        public string CurrentText
        {
            get { return (string)GetValue(CurrentTextProperty); }
            set { SetValue(CurrentTextProperty, value); }
        }

        public static readonly DependencyProperty CurrentTextProperty =
            DependencyProperty.Register("CurrentText", typeof(string),
                typeof(SplashScreen),
                new UIPropertyMetadata("",
                    new PropertyChangedCallback(CurrentNumberChanged)));

        private static void CurrentNumberChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
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
