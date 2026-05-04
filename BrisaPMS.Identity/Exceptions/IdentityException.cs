namespace BrisaPMS.Identity.Exceptions
{
    public class IdentityException : Exception
    {
        public IEnumerable<string> Errors { get; }
        public IdentityException(IEnumerable<string> errors)
            : base("One or more identity errors ocurred.")
        {
            Errors = errors;
        }

        public IdentityException(string errorMessage) : base(errorMessage) { }
    }
}
