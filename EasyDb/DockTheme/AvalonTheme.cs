using System;
using Xceed.Wpf.AvalonDock.Themes;

namespace EasyDb.DockTheme
{
    public class AvalonTheme : Theme
    {
        public override Uri GetResourceUri()
        {
            return new Uri(
                "/EasyDb;component/DockTheme/AvalonStylesDictionary.xaml",
                UriKind.Relative);
        }
    }
}