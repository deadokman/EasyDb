using System.ComponentModel;

namespace EDb.Interfaces
{
    /// <summary>
    /// Interface for datasource options
    /// Класс настроек для источника данных
    /// </summary>
    public abstract class EdbSourceOption
    {
        [Browsable(false)]
        public string OptionsDefinitionName { get; protected set; }
    }
}
