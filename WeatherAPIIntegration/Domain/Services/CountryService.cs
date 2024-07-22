namespace WeatherAPIIntegration.Domain.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidateCountry(string countryCode)
        {
            var response = await _httpClient.GetAsync($"https://restcountries.com/v3.1/alpha?codes={countryCode}");
            return response.IsSuccessStatusCode;
        }
    }
}
