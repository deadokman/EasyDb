using EasyDb.ViewModel.Interfaces;

namespace EasyDb.ViewModel.Panes
{
    using System.Windows;
    using System.Windows.Controls;

    using EasyDb.ViewModel.SqlEditors;

    /// <summary>
    /// Defines the <see cref="PanesTemplateSelector" />
    /// </summary>
    public class PanesTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the SqlEditorTemplate
        /// Шаблон отображения для таба
        /// </summary>
        public DataTemplate SqlEditorTemplate { get; set; }

        /// <summary>
        /// Gets or sets the SqlUiDiagrammTemplate
        /// Шаблон отображения для инструмента диаграмм
        /// </summary>
        public DataTemplate SqlUiDiagrammTemplate { get; set; }

        /// <summary>
        /// Get datatemplate for startup page
        /// </summary>
        public DataTemplate StartUpPageTemplate { get; set; }

        /// <summary>
        /// The SelectTemplate
        /// </summary>
        /// <param name="item">The item<see cref="object"/></param>
        /// <param name="container">The container<see cref="DependencyObject"/></param>
        /// <returns>The <see cref="DataTemplate"/></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SqlExecuterWindowViewModel)
            {
                return SqlEditorTemplate;
            }

            if (item is IStartUpPageViewModel)
            {
                return StartUpPageTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}