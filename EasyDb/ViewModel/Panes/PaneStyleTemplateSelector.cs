using System.Windows;
using System.Windows.Controls;

namespace EasyDb.ViewModel.Panes
{
    public class PaneStyleTemplateSelector : StyleSelector
    {
        public Style PaneWindowStyle { get; set; } 

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is PaneBaseViewModel)
            {
                return PaneWindowStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}
