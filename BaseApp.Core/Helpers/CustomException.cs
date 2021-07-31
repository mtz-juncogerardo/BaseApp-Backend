using System;

namespace BaseApp.Core.Helpers
{
    public static class CustomException
    {
        public static void Throw(string exceptionMessage, int httpStatusCode)
        {
            throw new Exception(exceptionMessage)
            {
                HResult = httpStatusCode
            };
        }
    }
}