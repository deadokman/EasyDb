using System.IO;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace EasyDb
{
    public static class LayoutManager
    {
        /// <summary>
        /// User layout file
        /// </summary>
        public const string LayoutFileName = "AvalonDock.Layout.config";

        /// <summary>
        /// Default layout file
        /// </summary>
        public const string DefaultLayoutFileName = "";

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

        public static void LoadLayout(DockingManager dockingManager)
        {
            //Загрузить лейаут если есть
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
    }
}
