using Newtonsoft.Json.Linq;

namespace WeatherAPIIntegration.Domain.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCountryCode(string country)
        {
            var response = await _httpClient.GetStringAsync($"https://restcountries.com/v3.1/name/{country}?fullText=true");
            var json = JArray.Parse(response);
            var cca2 = json[0]["cca2"].ToString();
            return cca2;
        }
    }
}
