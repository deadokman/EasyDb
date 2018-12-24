using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EasyDb.View;

namespace EasyDb.ViewModel
{
    public class DiagramDesignerViewModel : PaneBaseViewModel
    {
        public DiagramDesignerViewModel(string title)
            : base(title)
        {
            var view = new SqlQueryToolView();
            view.DataContext = this;
            ViewInstance = view;
        }

        public override UserControl ViewInstance { get; }
    }
}
