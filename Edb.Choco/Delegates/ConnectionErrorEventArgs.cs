namespace Edb.Environment.EventArgs
{
    using System;

    /// <summary>
    /// Defines the <see cref="ConnectionErrorEventArgs" />
    /// </summary>
    public class ConnectionErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Exception
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionErrorEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/></param>
        public ConnectionErrorEventArgs(Exception ex)
        {
            Exception = ex;
        }
    }
}
