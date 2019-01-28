namespace EasyDb.ViewModel.SqlEditors
{
    using System.Windows.Controls;

    using EasyDb.View;

    /// <summary>
    /// Defines the <see cref="SqlExecuterWindowViewModel" />
    /// </summary>
    public class SqlExecuterWindowViewModel : PaneBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExecuterWindowViewModel"/> class.
        /// </summary>
        /// <param name="title">The title<see cref="string"/></param>
        public SqlExecuterWindowViewModel(string title)
            : base(title)
        {
            var view = new SqlQueryToolView();
            view.DataContext = this;
            this.ViewInstance = view;
        }

        /// <summary>
        /// Gets the ViewInstance
        /// </summary>
        public override UserControl ViewInstance { get; }
    }
}