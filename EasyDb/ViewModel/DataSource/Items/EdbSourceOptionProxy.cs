using EDb.Interfaces;

namespace EasyDb.ViewModel.DataSource.Items
{
    /// <summary>
    /// ProxyClass for edb option
    /// </summary>
    public class EdbSourceOptionProxy : EdbSourceOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdbSourceOptionProxy"/> class.
        /// Options proxy
        /// </summary>
        /// <param name="edbOption">edb option</param>
        public EdbSourceOptionProxy(EdbSourceOption edbOption)
        {
            OptionSubject = edbOption;
        }

        /// <summary>
        /// Gets options subject
        /// </summary>
        public EdbSourceOption OptionSubject { get; private set; }

        /// <summary>
        /// Gets option defenition name
        /// </summary>
        public override string OptionsDefinitionName => OptionSubject?.OptionsDefinitionName;
    }
}
