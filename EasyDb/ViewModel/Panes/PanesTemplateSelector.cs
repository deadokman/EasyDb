using System.Windows;
using System.Windows.Controls;
using CSGO.Trader.ViewModel;
using EasyDb.ViewModel.SqlEditors;

namespace EasyDb.ViewModel.Panes
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Шаблон отображения для таба
        /// </summary>
        public DataTemplate SqlQueryToolTemplate { get; set; }

        /// <summary>
        /// Шаблон отображения для инструмента диаграмм
        /// </summary>
        public DataTemplate SqlUiDiagrammTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SqlExecuterWindowViewModel)
            {
                return SqlQueryToolTemplate;
            }

            if (item is DiagramDesignerViewModel)
            {
                return SqlUiDiagrammTemplate;
            }


            return base.SelectTemplate(item, container);
        }
    }
}
