using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edb.Environment.Interface
{
    using NuGet;

    /// <summary>
    /// Console log message listner interface
    /// </summary>
    public interface IChocoMessageListner
    {
        /// <summary>
        /// Notifies listner about messae dispatch
        /// </summary>
        /// <param name="level">Message level</param>
        /// <param name="message">MEssage text</param>
        /// <param name="ex">Exception if needed</param>
        void ChocoMessage(MessageLevel level, string message, Exception ex = null);
    }
}
