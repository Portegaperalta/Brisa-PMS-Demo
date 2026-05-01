namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePassword
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException() : base("Current password is incorrect") { }
    }
}