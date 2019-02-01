using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces.Validation
{
    /// <summary>
    /// Prepresents default validation rule
    /// </summary>
    public abstract class ValidationRuleBase
    {
        /// <summary>
        /// Validate current rule
        /// </summary>
        /// <returns>true if valid</returns>
        public abstract bool Validate();
    }
}
