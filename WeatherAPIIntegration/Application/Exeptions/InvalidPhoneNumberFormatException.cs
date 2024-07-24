namespace WeatherAPIIntegration.Application.Exeptions
{
    public class InvalidPhoneNumberFormatException : Exception
    {
        public InvalidPhoneNumberFormatException(string message) : base(message) { }
    }
}
