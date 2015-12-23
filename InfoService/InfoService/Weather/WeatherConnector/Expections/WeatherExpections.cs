#region Usings

using System;

#endregion

namespace WeatherConnector.Expections
{
    public class WeatherNoLocationIDExpection : Exception
    {
        public WeatherNoLocationIDExpection()
        {
        }

        public WeatherNoLocationIDExpection(string message)
            : base(message)
        {
        }

        public WeatherNoLocationIDExpection(string message, Exception inner)
            : base(message, inner)
        {
        }
    }


    public class WeatherNoPartnerIDOrLicenseKeyExpection : Exception
    {
        public WeatherNoPartnerIDOrLicenseKeyExpection()
        {
        }

        public WeatherNoPartnerIDOrLicenseKeyExpection(string message)
            : base(message)
        {
        }

        public WeatherNoPartnerIDOrLicenseKeyExpection(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}