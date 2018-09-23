using System.Windows;
using System.Windows.Media;
using MahApps.Metro;
using Brush = System.Drawing.Brush;

namespace CSGO.Trader.ViewModel.Settings
{
    /// <summary>
    /// Accent color application theme
    /// </summary>
    public class AccentColorMenuData
    {
        public string Name { get; set; }
        public SolidColorBrush BorderColorBrush { get; set; }
        public SolidColorBrush ColorBrush { get; set; }

        /// <summary>
        /// Apply this theme to application
        /// </summary>
        public void Apply()
        {
            this.DoChangeTheme(this);
        }

        protected virtual void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
        }
    }
}
