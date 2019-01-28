namespace EasyDb
{
    using System.IO;

    using Xceed.Wpf.AvalonDock;
    using Xceed.Wpf.AvalonDock.Layout.Serialization;

    /// <summary>
    /// Defines the <see cref="LayoutManager" />
    /// </summary>
    public static class LayoutManager
    {
        /// <summary>
        /// Default layout file
        /// </summary>
        public const string DefaultLayoutFileName = "";

        /// <summary>
        /// User layout file
        /// </summary>
        public const string LayoutFileName = "AvalonDock.Layout.config";

        /// <summary>
        /// The LoadLayout
        /// </summary>
        /// <param name="dockingManager">The dockingManager<see cref="DockingManager"/></param>
        public static void LoadLayout(DockingManager dockingManager)
        {
            // Загрузить лейаут если есть
            var file = Path.Combine(@".", LayoutFileName);
            if (File.Exists(file))
            {
                var layoutSerializer = new XmlLayoutSerializer(dockingManager);
                using (var stream = new StreamReader(file))
                {
                    layoutSerializer.Deserialize(stream);
                }
            }
        }

        /// <summary>
        /// The StoreLayout
        /// </summary>
        /// <param name="dockingManager">The dockingManager<see cref="DockingManager"/></param>
        public static void StoreLayout(DockingManager dockingManager)
        {
            var xmlLayout = new XmlLayoutSerializer(dockingManager);
            var file = Path.Combine(@".", LayoutFileName);
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            xmlLayout.Serialize(file);
        }
    }
}