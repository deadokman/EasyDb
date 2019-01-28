namespace EasyDb.DockTheme
{
    using System;

    using Xceed.Wpf.AvalonDock.Themes;

    /// <summary>
    /// Defines the <see cref="AvalonTheme" />
    /// </summary>
    public class AvalonTheme : Theme
    {
        /// <summary>
        /// The GetResourceUri
        /// </summary>
        /// <returns>The <see cref="Uri"/></returns>
        public override Uri GetResourceUri()
        {
            return new Uri("/EasyDb;component/DockTheme/AvalonStylesDictionary.xaml", UriKind.Relative);
        }
    }
}