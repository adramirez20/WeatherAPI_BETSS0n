namespace WeatherAPIIntegration.Application.Exeptions
{
    public class InvalidCountryCodeException : Exception
    {
        public InvalidCountryCodeException(string message) : base(message) { }
    }
}
