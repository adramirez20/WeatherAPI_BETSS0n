namespace WeatherAPIIntegration.Domain.Services
{
    public interface ICountryService
    {
        public Task<string> GetCountryCode(string country);
    }
}
