namespace WeatherAPIIntegration.Application.Exeptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
    }
}
