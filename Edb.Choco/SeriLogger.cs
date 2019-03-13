namespace Edb.Environment
{
    namespace ChocolateyGui
    {
        using chocolatey.infrastructure.logging;
        using System;

        /// <summary>
        /// Defines the <see cref="SerilogLogger" />
        /// </summary>
        public class SerilogLogger : ILog
        {
            /// <summary>
            /// Defines the _logger
            /// </summary>
            private readonly Autofac.Extras.NLog.ILogger _logger;

            /// <summary>
            /// Defines the _context
            /// </summary>
            private string _context;

            /// <summary>
            /// Initializes a new instance of the <see cref="SerilogLogger"/> class.
            /// </summary>
            /// <param name="logger">The logger<see cref="Autofac.Extras.NLog.ILogger"/></param>
            public SerilogLogger(Autofac.Extras.NLog.ILogger logger)
            {
                this._logger = logger;
            }

            /// <summary>
            /// The InitializeFor
            /// </summary>
            /// <param name="loggerName">The loggerName<see cref="string"/></param>
            public void InitializeFor(string loggerName)
            {
                this._context = loggerName;
            }

            /// <summary>
            /// The Debug
            /// </summary>
            /// <param name="message">The message<see cref="string"/></param>
            /// <param name="formatting">The formatting<see cref="object[]"/></param>
            public void Debug(string message, params object[] formatting)
            {
                this._logger.Debug(message, formatting);
            }

            /// <summary>
            /// The Debug
            /// </summary>
            /// <param name="message">The message<see cref="Func{string}"/></param>
            public void Debug(Func<string> message)
            {
                this._logger.Debug(message());
            }

            /// <summary>
            /// The Info
            /// </summary>
            /// <param name="message">The message<see cref="string"/></param>
            /// <param name="formatting">The formatting<see cref="object[]"/></param>
            public void Info(string message, params object[] formatting)
            {
                this._logger.Info(message, formatting);
            }

            /// <summary>
            /// The Info
            /// </summary>
            /// <param name="message">The message<see cref="Func{string}"/></param>
            public void Info(Func<string> message)
            {
                this._logger.Info(message());
            }

            /// <summary>
            /// The Warn
            /// </summary>
            /// <param name="message">The message<see cref="string"/></param>
            /// <param name="formatting">The formatting<see cref="object[]"/></param>
            public void Warn(string message, params object[] formatting)
            {
                this._logger.Warn(message, formatting);
            }

            /// <summary>
            /// The Warn
            /// </summary>
            /// <param name="message">The message<see cref="Func{string}"/></param>
            public void Warn(Func<string> message)
            {
                this._logger.Warn(message());
            }

            /// <summary>
            /// The Error
            /// </summary>
            /// <param name="message">The message<see cref="string"/></param>
            /// <param name="formatting">The formatting<see cref="object[]"/></param>
            public void Error(string message, params object[] formatting)
            {
                this._logger.Error(message, formatting);
            }

            /// <summary>
            /// The Error
            /// </summary>
            /// <param name="message">The message<see cref="Func{string}"/></param>
            public void Error(Func<string> message)
            {
                this._logger.Error(message());
            }

            /// <summary>
            /// The Fatal
            /// </summary>
            /// <param name="message">The message<see cref="string"/></param>
            /// <param name="formatting">The formatting<see cref="object[]"/></param>
            public void Fatal(string message, params object[] formatting)
            {
                this._logger.Fatal(message, formatting);
            }

            /// <summary>
            /// The Fatal
            /// </summary>
            /// <param name="message">The message<see cref="Func{string}"/></param>
            public void Fatal(Func<string> message)
            {
                this._logger.Fatal(message());
            }

            /// <summary>
            /// The Intercept
            /// </summary>
            /// <param name="interceptor">The interceptor<see cref="Action{LogMessage}"/></param>
            /// <returns>The <see cref="IDisposable"/></returns>
            public IDisposable Intercept(Action<LogMessage> interceptor)
            {
                return new InterceptMessages(this, interceptor);
            }
        }
    }
}
