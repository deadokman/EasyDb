using System.Windows.Controls;
using EasyDb.View;

namespace EasyDb.ViewModel.SqlEditors
{
    public class SqlExecuterWindowViewModel : PaneBaseViewModel
    {
        public SqlExecuterWindowViewModel(string title) 
            : base(title)
        {
            var view = new SqlQueryToolView();
            view.DataContext = this;
            ViewInstance = view;
        }

        public override UserControl ViewInstance { get; }
    }
}
