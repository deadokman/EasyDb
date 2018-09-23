//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CSGO.Trader.PluginFunctional
//{
//    /// <summary>
//    /// Plugin load information
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Class)]
//    public class PluginConfigurationsAttribute : Attribute
//    {
//        /// <summary>
//        /// Создать экземпляр аттрибута с параметрами плагина
//        /// </summary>
//        /// <param name="guid">Guid плагина, для генерации предалагается использовать генератор Visual Studio, Tools -> CreateTransparantProxy GUID (Registry Format) </param>
//        /// <param name="title">Название плагина</param>
//        public PluginConfigurationsAttribute(string guid, string title)
//        {
//            PluginGuid = new Guid(guid);
//            PluginTitle = title;
//        }

//        public string PluginTitle { get; private set; }

//        /// <summary>
//        /// Guid плагина, использующийся для его идентификации
//        /// </summary>
//        public Guid PluginGuid { get; private set; }

//    }
//}
