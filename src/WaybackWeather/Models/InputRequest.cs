using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public string LocationResult { get; set; }

        public void GetLatLong()
        {
            RestClient client = new RestClient("https://api.mapbox.com");
            Location = Location.Replace(" ", "+");    //url seems to prefer + instead of space
            RestRequest request = new RestRequest("/geocoding/v5/mapbox.places/" + Location + ".json?access_token="+ EnvironmentVariables.MapBoxToken, Method.GET);
            RestResponse response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            object[] features = JsonConvert.DeserializeObject<object[]>(jsonResponse["features"].ToString());
            JObject firstElement = JsonConvert.DeserializeObject<JObject>(features[0].ToString());
            string[] center = JsonConvert.DeserializeObject<string[]>(firstElement["center"].ToString());

            Long = float.Parse(center[0]);
            Lat = float.Parse(center[1]);
            LocationResult = firstElement["place_name"].ToString();
        }

        public Dictionary<string, string> GetWeather()
        {
            RestClient client = new RestClient("https://api.darksky.net");
            RestRequest request = new RestRequest($"/forecast/{EnvironmentVariables.DarkSkyKey}/{Lat},{Long},{Date}", Method.GET);
            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            try
            {
                JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
                JObject daily = JsonConvert.DeserializeObject<JObject>(jsonResponse["daily"].ToString());
                JObject[] data = JsonConvert.DeserializeObject<JObject[]>(daily["data"].ToString());
                JObject firstElement = JsonConvert.DeserializeObject<JObject>(data[0].ToString());
                return new Dictionary<string, string>
                {
                    ["summary"] = firstElement["summary"].ToString(),
                    ["temperatureMin"] = firstElement["temperatureMin"].ToString(),
                    ["temperatureMax"] = firstElement["temperatureMax"].ToString(),
                    ["foundLocation"] = LocationResult
                };
            }
            catch
            {
                return new Dictionary<string, string>()
                {
                    //Force summary to null in order to check validation on front end
                    ["summary"] = null
                };
            }
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
