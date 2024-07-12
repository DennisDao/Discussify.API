namespace Domain.Exception
{
    public class UserNotFoundDomainException : System.Exception
    {
        public UserNotFoundDomainException()
        { }

        public UserNotFoundDomainException(string message)
            : base(message)
        { }

        public UserNotFoundDomainException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
