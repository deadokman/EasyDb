namespace EasyDb
{
    using System;

    using NLog;

    /// <summary>
    /// Defines the <see cref="Helpers" />
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// The LogAndRethrow
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger"/></param>
        /// <param name="text">The text<see cref="string"/></param>
        /// <param name="ex">The ex<see cref="Exception"/></param>
        public static void LogAndRethrow(ILogger logger, string text, Exception ex)
        {
            var newEx = new Exception(text, ex);
            logger.Error(newEx);
            throw newEx;
        }
    }
}