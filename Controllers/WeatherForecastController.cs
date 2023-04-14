using Individual_Project_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Individual_Project_API.Controllers
{
    public class WeatherForecastController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public WeatherForecastController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet("GetWeatherForecast/{city}")]
        public async Task<string> Get(string city) 
        {
            string errorString;

            var httpClient = httpClientFactory.CreateClient();

            var url = $"https://api.weatherapi.com/v1/current.json?key=0e75156b69a14c10819131239231204&q={city}";

            try
            {
                var response = await httpClient.GetStringAsync(url);

                var weather = JsonConvert.DeserializeObject<Rootobject>(response);

                Console.WriteLine($"The weather is {weather.location.country}");

                return response;
            }
            catch (Exception e)
            {
                errorString = $"There was an error retrieving the weather: {e.Message}";
            }

            return errorString;
        }
/*
        public IActionResult Index()
        {
            return View();
        }*/
    }
}
