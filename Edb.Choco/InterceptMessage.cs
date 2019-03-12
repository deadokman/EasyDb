using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chocolatey.infrastructure.logging;
using Edb.Environment.ChocolateyGui;
using Edb.Environment.Model;

namespace Edb.Environment
{
    public class InterceptMessages : IDisposable
    {
        private SerilogLogger _logger;

        public InterceptMessages(SerilogLogger logger, Action<LogMessage> interceptor)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            _logger = null;
        }
    }
}
