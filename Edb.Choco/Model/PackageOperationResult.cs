using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edb.Environment.Model
{
    public class PackageOperationResult
    {
        public static readonly PackageOperationResult SuccessfulCached = new PackageOperationResult
        {
            Successful = true
        };

        public bool Successful { get; set; }

        public string[] Messages { get; set; } = new string[0];

        public Exception Exception { get; set; }
    }
}
