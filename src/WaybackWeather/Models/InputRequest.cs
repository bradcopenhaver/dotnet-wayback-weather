using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaybackWeather.Models
{
    public class InputRequest
    {
        public string Location { get; set; }
        public float Lat { get; set; }
        public float Long { get; set; }
        public long Date { get; set; }

        public static List<float> GetLatLong(string location)
        {
            RestClient client = new RestClient("");
            RestRequest request = new RestRequest("/geocoding/v5/mapbox.places/", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator("", "");
            RestResponse response = 
        }
    }
}
