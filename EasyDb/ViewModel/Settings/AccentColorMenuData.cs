namespace CSGO.Trader.ViewModel.Settings
{
    using System.Windows;
    using System.Windows.Media;

    using MahApps.Metro;

    /// <summary>
    /// Accent color application theme
    /// </summary>
    public class AccentColorMenuData
    {
        /// <summary>
        /// Gets or sets the BorderColorBrush
        /// </summary>
        public SolidColorBrush BorderColorBrush { get; set; }

        /// <summary>
        /// Gets or sets the ColorBrush
        /// </summary>
        public SolidColorBrush ColorBrush { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Apply this theme to application
        /// </summary>
        public void Apply()
        {
            DoChangeTheme(this);
        }

        /// <summary>
        /// The DoChangeTheme
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        protected virtual void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
        }
    }
}