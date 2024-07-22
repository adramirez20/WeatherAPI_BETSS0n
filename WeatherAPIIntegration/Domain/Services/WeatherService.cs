﻿using Newtonsoft.Json.Linq;
using WeatherAPIIntegration.DTOs;

namespace WeatherAPIIntegration.Domain.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherDto> GetWeatherAsync(string location)
        {
            var response = await _httpClient.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?q={location}&appid=YOUR_API_KEY");
            var weatherData = JObject.Parse(response);

            return new WeatherDto
            {
                Location = location,
                Temperature = weatherData["main"]["temp"].ToString(),
                Condition = weatherData["weather"][0]["description"].ToString()
            };
        }
    }
}