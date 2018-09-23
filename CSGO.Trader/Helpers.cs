using System;
using NLog;

namespace EasyDb
{
    public static class Helpers
    {
        public static void LogAndRethrow(ILogger logger, string text, Exception ex)
        {
            var newEx = new Exception(text, ex);
            logger.Error(newEx);
            throw newEx;
        }
    }
}
