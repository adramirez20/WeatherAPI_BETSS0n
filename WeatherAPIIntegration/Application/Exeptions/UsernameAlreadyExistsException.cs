namespace WeatherAPIIntegration.Application.Exeptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException(string message) : base(message) { }
    }
}
