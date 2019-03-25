namespace EasyDb.Commands
{
    using System;
    using System.Windows.Input;

#pragma warning disable SA1402 // File may only contain a single class
                              /// <summary>
                              /// Defines the <see cref="EDbCommand{T}" />
                              /// </summary>
                              /// <typeparam name="T">Command argument</typeparam>
    public class EDbCommand<T> : ICommand
#pragma warning restore SA1402 // File may only contain a single class
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EDbCommand{T}"/> class.
        /// </summary>
        /// <param name="invokeCommand">command invocator</param>
        public EDbCommand(Action<T> invokeCommand)
        {
            InvokeCommand = invokeCommand;
        }

        /// <summary>
        /// Defines the CanExecuteChanged
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the _invokeCommand
        /// </summary>
        protected Action<T> InvokeCommand { get; set; }

        /// <summary>
        /// Set can execute
        /// </summary>
        public void SetCanExecute()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// The CanExecute
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public virtual bool CanExecute(object parameter)
        {
            T castType = parameter as T;
            return InvokeCommand != null && castType != null;
        }

        /// <summary>
        /// The Execute
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        public void Execute(object parameter)
        {
            InvokeCommand.Invoke((T)parameter);
        }
    }

    /// <summary>
    /// Empty yype comamnd
    /// </summary>
    public class EDbCommand : EDbCommand<object>
    {
        private readonly Action _invocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EDbCommand"/> class.
        /// Base ctor
        /// </summary>
        /// <param name="invocator">Invocator</param>
        public EDbCommand(Action invocator)
            : base((v) => { invocator.Invoke(); })
        {
            _invocator = invocator;
        }

        /// <summary>
        /// The CanExecute
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public override bool CanExecute(object parameter)
        {
            return InvokeCommand != null && _invocator != null;
        }
    }
}
