namespace WeatherAPIIntegration.Domain.Services
{
    public interface ICountryService
    {
        public Task<bool> ValidateCountry(string countryCode);
    }
}
