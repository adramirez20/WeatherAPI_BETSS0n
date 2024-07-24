namespace WeatherAPIIntegration.Application.Services
{
    public interface ICountryService
    {
        public Task<string> GetCountryCode(string country);
    }
}
