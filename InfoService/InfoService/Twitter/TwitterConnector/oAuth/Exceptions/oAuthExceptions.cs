using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterConnector.oAuth.Exceptions
{
    public class OAuthHttpWebRequestException : Exception
    {
        public int RateLimit { get; set; }
        public int LimitRemaining { get; set; }
        public DateTime RateReset { get; set; }
        public bool RateLimitsFound { get; set; }

        public OAuthHttpWebRequestException()
        {
            RateLimit = 0;
            LimitRemaining = 0;
            RateReset = DateTime.MinValue;
            RateLimitsFound = false;
        }
        public OAuthHttpWebRequestException(string message, Exception inner) : base(message, inner)
        {
            RateLimit = 0;
            LimitRemaining = 0;
            RateReset = DateTime.MinValue;
            RateLimitsFound = false;
        }
        public OAuthHttpWebRequestException(int rateLimit, int limitRemaining, DateTime rateReset)
        {
            RateLimit = rateLimit;
            LimitRemaining = limitRemaining;
            RateReset = rateReset;
            RateLimitsFound = true;
        }

        public OAuthHttpWebRequestException(string message, int rateLimit, int limitRemaining, DateTime rateReset)
            : base(message)
        {
            RateLimit = rateLimit;
            LimitRemaining = limitRemaining;
            RateReset = rateReset;
            RateLimitsFound = true;
        }

        public OAuthHttpWebRequestException(string message, int rateLimit, int limitRemaining, DateTime rateReset, Exception inner)
            : base(message, inner)
        {
            RateLimit = rateLimit;
            LimitRemaining = limitRemaining;
            RateReset = rateReset;
            RateLimitsFound = true;
        }
    }
}
