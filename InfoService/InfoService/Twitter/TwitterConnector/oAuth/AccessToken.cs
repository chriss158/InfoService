using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterConnector.OAuth
{
    /// <summary>
    /// Stands for access token
    /// </summary>
    public class AccessToken:IToken 
    {
        private String _tokenValue;
        private String _tokenSecret;

        /// <summary>
        /// Access token value
        /// </summary>
        public String TokenValue
        {
            get
            {
                return _tokenValue;
            }
        }

        /// <summary>
        /// Access token secret
        /// </summary>
        public String TokenSecret
        {
            get
            {
                return _tokenSecret;
            }
        }

        /// <summary>
        /// Construct Access Token
        /// </summary>
        /// <param name="tokenValue">Access token value</param>
        /// <param name="tokenSecret">Access token secret</param>
        public AccessToken(
            String tokenValue,
            String tokenSecret
            )
        {
            _tokenValue = tokenValue;
            _tokenSecret = tokenSecret;
        }

    }
}
