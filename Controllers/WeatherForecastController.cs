using Individual_Project_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Individual_Project_API.Controllers
{
    public class WeatherForecastController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMemoryCache memoryCache;

        public WeatherForecastController(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            this.httpClientFactory = httpClientFactory;
            this.memoryCache = memoryCache;
        }

        [EnableCors("weatherPolicy")]
        [HttpGet("GetWeatherForecast/{city}")]
        public async Task<string> Get(string city) 
        {
            string errorString;

            var httpClient = httpClientFactory.CreateClient();

            var url = $"https://api.weatherapi.com/v1/forecast.json?key=0e75156b69a14c10819131239231204&q={city}&days=10";

            var weatherForecastList = memoryCache.Get<List<WeatherForecast>>("Weather");

            if (weatherForecastList != null && city == weatherForecastList[0].name)
            {
                var weatherForecastJson = JsonConvert.SerializeObject(weatherForecastList);

                return weatherForecastJson;
            }

            try
            {
                weatherForecastList = new List<WeatherForecast>();

                var response = await httpClient.GetStringAsync(url);

                var weatherObject = JsonConvert.DeserializeObject<Rootobject>(response);

                foreach (var dailyForecast in weatherObject.forecast.forecastday)
                {
                        var weatherForecast = new WeatherForecast();

                        weatherForecast.name = weatherObject.location.name;
                        weatherForecast.region = weatherObject.location.region;
                        weatherForecast.country = weatherObject.location.country;
                        weatherForecast.date = dailyForecast.date;
                        weatherForecast.mintemp_c = dailyForecast.day.mintemp_c;
                        weatherForecast.maxtemp_c = dailyForecast.day.maxtemp_c;
                        weatherForecast.avgtemp_c = dailyForecast.day.avgtemp_c;
                        weatherForecast.maxwind_kph = dailyForecast.day.maxwind_kph;
                        weatherForecast.avghumidity = dailyForecast.day.avghumidity;

                        weatherForecastList.Add(weatherForecast);
                }

                memoryCache.Set("Weather", weatherForecastList, TimeSpan.FromDays(1));

                var weatherForecastJson = JsonConvert.SerializeObject(weatherForecastList);

                return weatherForecastJson;
            }
            catch (Exception e)
            {
                errorString = $"There was an error retrieving the weather: {e.Message}";
            }

            return errorString;
        }
    }
}
