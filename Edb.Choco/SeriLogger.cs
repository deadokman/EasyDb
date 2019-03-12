using NuGet;
using System;
using chocolatey.infrastructure.logging;

namespace Edb.Environment
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright company="Chocolatey" file="SerilogLogger.cs">
    //   Copyright 2014 - Present Rob Reynolds, the maintainers of Chocolatey, and RealDimensions Software, LLC
    // </copyright>
    // --------------------------------------------------------------------------------------------------------------------

    namespace ChocolateyGui
    {
        public class SerilogLogger : ILog
        {
            private readonly ILogger _logger;
            //private Action<Models.LogMessage> _interceptor;
            private string _context;
            // private IProgressService _progressService;

            public SerilogLogger(ILogger logger)
            {
                _logger = logger;
                // _progressService = progressService;
            }

            //public IDisposable Intercept(Action<Models.LogMessage> interceptor)
            //{
            //    return new InterceptMessages(this, interceptor);
            //}

            public void InitializeFor(string loggerName)
            {
                _context = loggerName;
            }

            public void Debug(string message, params object[] formatting)
            {
                _logger.Log(MessageLevel.Debug, message, formatting);

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Debug,
                //    Message = string.Format(message, formatting)
                //};

                //_interceptor?.Invoke(logMessage);
            }

            public void Debug(Func<string> message)
            {
                _logger.Log(MessageLevel.Debug, message());

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Debug,
                //    Message = message()
                //};

                //_interceptor?.Invoke(logMessage);
            }

            public void Info(string message, params object[] formatting)
            {
                _logger.Log(MessageLevel.Info, message, formatting);

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Info,
                //    Message = string.Format(message, formatting)
                //};

                //_interceptor?.Invoke(logMessage);
                //_progressService.WriteMessage(logMessage.Message, PowerShellLineType.Output);
            }

            public void Info(Func<string> message)
            {
                _logger.Log(MessageLevel.Info, message());

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Info,
                //    Message = message()
                //};

                //_interceptor?.Invoke(logMessage);
               // _progressService.WriteMessage(logMessage.Message, PowerShellLineType.Output);
            }

            public void Warn(string message, params object[] formatting)
            {
                _logger.Log(MessageLevel.Warning, message, formatting);

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Warn,
                //    Message = string.Format(message, formatting)
                //};

                //_interceptor?.Invoke(logMessage);
                //_progressService.WriteMessage(logMessage.Message, PowerShellLineType.Warning);
            }

            public void Warn(Func<string> message)
            {
                _logger.Log(MessageLevel.Warning, message());

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Warn,
                //    Message = message()
                //};

                //_interceptor?.Invoke(logMessage);
                //_progressService.WriteMessage(logMessage.Message, PowerShellLineType.Warning);
            }

            public void Error(string message, params object[] formatting)
            {
                _logger.Log(MessageLevel.Error, message, formatting);

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Error,
                //    Message = string.Format(message, formatting)
                //};

                //_interceptor?.Invoke(logMessage);
                //_progressService.WriteMessage(logMessage.Message, PowerShellLineType.Error);
            }

            public void Error(Func<string> message)
            {
                _logger.Log(MessageLevel.Error, message());

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Error,
                //    Message = message()
                //};

                //_interceptor?.Invoke(logMessage);
                // _progressService.WriteMessage(logMessage.Message, PowerShellLineType.Error);
            }

            public void Fatal(string message, params object[] formatting)
            {
                _logger.Log(MessageLevel.Fatal, message, formatting);

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Fatal,
                //    Message = string.Format(message, formatting)
                //};

                //_interceptor?.Invoke(logMessage);
                // _progressService.WriteMessage(logMessage.Message, PowerShellLineType.Error);
            }

            public void Fatal(Func<string> message)
            {
                _logger.Log(MessageLevel.Fatal, message());

                //var logMessage = new Models.LogMessage
                //{
                //    Context = _context,
                //    LogLevel = LogLevel.Fatal,
                //    Message = message()
                //};

                //_interceptor?.Invoke(logMessage);
                // _progressService.WriteMessage(logMessage.Message, PowerShellLineType.Error);
            }

            public IDisposable Intercept(Action<LogMessage> interceptor)
            {
                return new InterceptMessages(this, interceptor);
            }

            //public class InterceptMessages : IDisposable
            //{
            //    private readonly SerilogLogger _logger;

            //    public InterceptMessages(SerilogLogger logger, Action<Models.LogMessage> interceptor)
            //    {
            //        _logger = logger;
            //        logger._interceptor = interceptor;
            //    }

            //    public void Dispose()
            //    {
            //        _logger._interceptor = null;
            //    }
            //}
        }
    }
}
