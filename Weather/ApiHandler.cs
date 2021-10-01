using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    class ApiHandler
    {
        public HttpClient Client { get; set; }

        public ApiHandler()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<string>> GetAllCitiesAsync()
        {
            using HttpResponseMessage response = await Client.GetAsync("https://countriesnow.space/api/v0.1/countries");

            if (response.IsSuccessStatusCode)
            {
                var output = new List<string>();
                var countries = await response.Content.ReadAsAsync<CountriesDataListModel>();

                foreach(var country in countries.Data)
                {
                    foreach(var city in country.Cities)
                    {
                        output.Add(city);
                    }
                }

                return output;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<WeatherModel> GetCurrentWeather(string city, string apiKey)
        {
            var request = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
            using HttpResponseMessage response = await Client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var weather = await response.Content.ReadAsAsync<WeatherModel>();
                return weather;
            }
            else
            {
                // some cities might not exist in the weather api database, so we need to skip them instead of throwing an exception
                return null; 
            }
        }
    }
}
