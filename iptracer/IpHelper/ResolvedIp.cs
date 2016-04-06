using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iptracer
{
    class ResolvedIp
    {
        #region Private properties

        private string statusCode;
        private IPAddress IpAddress;
        private string countryCode;
        private string regionName;
        private string cityName;
        private string zipCode;
        private string latitude;
        private string longitude;
        private string timeZone;

        public string StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }

        public IPAddress IpAddress1
        {
            get { return IpAddress; }
            set { IpAddress = value; }
        }

        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }

        public string RegionName
        {
            get { return regionName; }
            set { regionName = value; }
        }

        public string CityName
        {
            get { return cityName; }
            set { cityName = value; }
        }

        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public string TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }

        #endregion Private properties

        #region Public methods

        public ResolvedIp()
        {
            
        }
        #endregion
    }
    /*
    {
	"statusCode" : "OK",
	"statusMessage" : "",
	"ipAddress" : "216.58.213.234",
	"countryCode" : "US",
	"countryName" : "United States",
	"regionName" : "California",
	"cityName" : "Mountain View",
	"zipCode" : "94043",
	"latitude" : "37.406",
	"longitude" : "-122.079",
	"timeZone" : "-07:00"
}*/
}
