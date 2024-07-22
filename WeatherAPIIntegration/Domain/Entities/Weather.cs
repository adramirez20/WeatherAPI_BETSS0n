namespace WeatherAPIIntegration.Domain.Entities
{
    public class Weather
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string WeatherDesc { get; set; }
        public DateTime Date { get; set; }
    }
}
