using System.Windows;
using System.Windows.Controls;
using CSGO.Trader.ViewModel;

namespace EasyDb.ViewModel.Panes
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PluginMarketplaceTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is PluginBaseViewModel)
            {
                return PluginMarketplaceTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
