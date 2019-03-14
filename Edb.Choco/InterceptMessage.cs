using System;

using chocolatey.infrastructure.logging;
using Edb.Environment.ChocolateyGui;

namespace Edb.Environment
{
    public class InterceptMessages : IDisposable
    {
        private ChocolateyLoggerWrapper _logger;

        public InterceptMessages(ChocolateyLoggerWrapper logger, Action<LogMessage> interceptor)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            _logger = null;
        }
    }
}
