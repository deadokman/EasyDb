namespace EasyDb.Model
{
    using EDb.Interfaces;
    using EDb.Interfaces.Options;

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
            this.OptionSubject = edbOption;
        }

        /// <summary>
        /// Gets option defenition name
        /// </summary>
        public override string OptionsDefinitionName => this.OptionSubject?.OptionsDefinitionName;

        /// <summary>
        /// Gets options subject
        /// </summary>
        protected EdbSourceOption OptionSubject { get; private set; }

        /// <summary>
        /// Return module option definition
        /// </summary>
        /// <returns>Module options definition</returns>
        public override ModuleOptionDefinition ToOptionDefinition()
        {
            return this.OptionSubject.ToOptionDefinition();
        }
    }
}
