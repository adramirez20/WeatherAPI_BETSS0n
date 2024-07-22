namespace WeatherAPIIntegration.Domain.Services
{
    public static class UsernameGenerator
    {
        public static string Generate(string firstName, string lastName)
        {
            return $"{firstName.Substring(0, 1).ToLower()}{lastName.ToLower()}{new Random().Next(10000, 99999)}";
        }
    }
}
