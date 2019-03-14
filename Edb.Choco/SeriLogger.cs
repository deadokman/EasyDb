namespace Edb.Environment
{
    namespace ChocolateyGui
    {
        using chocolatey.infrastructure.logging;
        using System;
        using System.Collections.Generic;

        using Edb.Environment.Interface;

        /// <summary>
        /// Defines the <see cref="ChocolateyLoggerWrapper" />
        /// </summary>
        public class ChocolateyLoggerWrapper : ILog
        {
            /// <summary>
            /// Defines the _logger
            /// </summary>
            private readonly Autofac.Extras.NLog.ILogger _logger;

            /// <summary>
            /// Defines the _context
            /// </summary>
            private List<IChocoMessageListner> _listners;

            /// <summary>
            /// Initializes a new instance of the <see cref="ChocolateyLoggerWrapper"/> class.
            /// </summary>
            /// <param name="logger">The logger<see cref="Autofac.Extras.NLog.ILogger"/></param>
            public ChocolateyLoggerWrapper(Autofac.Extras.NLog.ILogger logger)
            {
                this._logger = logger;
                _listners = new List<IChocoMessageListner>();
            }

            /// <summary>
            /// The InitializeFor
            /// </summary>
            /// <param name="loggerName">The loggerName<see cref="string"/></param>
            public void InitializeFor(string loggerName)
            {
                
            }

            /// <summary>
            /// Register chocolatey log listner
            /// </summary>
            /// <param name="listner">Listner instance</param>
            public void RegisterListner(IChocoMessageListner listner)
            {
                if (!this._listners.Contains(listner))
                {
                    this._listners.Add(listner);
                }
            }

            /// <summary>
            /// Register chocolatey log listner
            /// </summary>
            /// <param name="listner">Listner instance</param>
            public void UnregisterListner(IChocoMessageListner listner)
            {
                if (!this._listners.Contains(listner))
                {
                    this._listners.Remove(listner);
                }
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
