using System.Windows;
using System.Windows.Controls;

namespace EasyDb.ViewModel.Panes
{
    public class PaneStyleTemplateSelector : StyleSelector
    {
        public Style MarketplacePluginStyle { get; set; } 

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is PluginBaseViewModel)
            {
                return MarketplacePluginStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}
