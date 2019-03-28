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

namespace EasyDb.View.StartupPage
{
    /// <summary>
    /// Interaction logic for StartUpPageControll.xaml
    /// </summary>
    public partial class StartUpPageControll : UserControl
    {
        private GridLength _leftColLength;

        private GridLength _rightColLength;

        private GridLength _expanderColLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpPageControll"/> class.
        /// </summary>
        public StartUpPageControll()
        {
            InitializeComponent();
            _leftColLength = this.LeftColumn.Width;
            _rightColLength = this.RightColumn.Width;
            _expanderColLength = this.ExpanderColumn.Width;
        }

        private void PatchnotesExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            LeftColumn.Width = new GridLength(0, GridUnitType.Auto);
            RightColumn.Width = new GridLength(0, GridUnitType.Auto);
            ExpanderColumn.Width = new GridLength(1, GridUnitType.Star);
        }

        private void PatchnotesExpander_OnCollapsed(object sender, RoutedEventArgs e)
        {
            LeftColumn.Width = _leftColLength;
            RightColumn.Width = _rightColLength;
            ExpanderColumn.Width = _expanderColLength;
        }
    }
}
