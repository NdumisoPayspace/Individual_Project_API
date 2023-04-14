namespace Individual_Project_API.Models
{
    public class WeatherForecast
    {
        public string name { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public string date { get; set; }
        public float mintemp_c { get; set; }
        public float maxtemp_c { get; set; }
        public float avgtemp_c { get; set; }
        public float maxwind_kph { get; set; }
        public float avghumidity { get; set; }
    }
}

