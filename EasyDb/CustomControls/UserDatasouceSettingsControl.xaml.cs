using System.Windows;
using System.Windows.Controls;

namespace EasyDb.CustomControls
{
    /// <summary>
    /// Interaction logic for UserDatasouceSettingsControl.xaml
    /// </summary>
    public partial class UserDatasouceSettingsControl : UserControl
    {
        public UserDatasouceSettingsControl()
        {
            InitializeComponent();
        }



        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(object), typeof(UserDatasouceSettingsControl), new PropertyMetadata(null, SetObjectPropsDisplay));

        private static void SetObjectPropsDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
