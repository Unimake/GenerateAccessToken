using System;
using System.Net;

namespace Unimake.GenerateAccessToken
{
    internal class ApiException : Exception
    {
        #region Public Fields

        public HttpStatusCode StatusCode;

        public string Text;

        #endregion Public Fields

        #region Public Constructors

        public ApiException(string message, HttpStatusCode statusCode, string text)
            : base(message)
        {
            StatusCode = statusCode;
            Text = text;
        }

        public ApiException(string message, HttpStatusCode statusCode, string text, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Text = text;
        }

        #endregion Public Constructors
    }
}