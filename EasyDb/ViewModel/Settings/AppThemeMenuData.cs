namespace CSGO.Trader.ViewModel.Settings
{
    using System.Windows;

    using MahApps.Metro;

    /// <summary>
    /// Base app theme class
    /// </summary>
    public class AppThemeMenuData : AccentColorMenuData
    {
        /// <summary>
        /// The DoChangeTheme
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        protected override void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
        }
    }
}