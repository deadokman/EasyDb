using System.Windows;
using MahApps.Metro;

namespace CSGO.Trader.ViewModel.Settings
{
    /// <summary>
    /// Base app theme class
    /// </summary>
    public class AppThemeMenuData : AccentColorMenuData
    {
        protected override void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
        }
    }
}
