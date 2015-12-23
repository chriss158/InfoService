using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterConnector.OAuth
{
    /// <summary>
    /// Stands for request token.
    /// </summary>
    public class RequestToken:IToken 
    {
        private String _tokenValue;
        private String _tokenSecret;

        /// <summary>
        /// Request token value
        /// </summary>
        public String TokenValue
        {
            get
            {
                return _tokenValue;
            }
        }

        /// <summary>
        /// Request token secret
        /// </summary>
        public String TokenSecret
        {
            get
            {
                return _tokenSecret;
            }
        }

        /// <summary>
        /// Construct request token
        /// </summary>
        /// <param name="tokenValue">Request token value</param>
        /// <param name="tokenSecret">Request token secret</param>
        public RequestToken(String tokenValue, String tokenSecret)
        {
            _tokenValue = tokenValue;
            _tokenSecret = tokenSecret;
        }

    }
}
