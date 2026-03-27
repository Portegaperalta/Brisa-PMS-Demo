namespace BrisaPMS.Domain.Exceptions.InvalidValueExceptions
{
    public class InvalidEmailAddressException : Exception
    {
        public InvalidEmailAddressException() : base("The email must be a valid email address") { }
    }
}
