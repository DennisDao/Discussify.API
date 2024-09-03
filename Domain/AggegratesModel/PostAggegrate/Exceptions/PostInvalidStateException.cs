namespace Domain.AggegratesModel.PostAggegrate.Exceptions
{
    public class PostInvalidStateException : Exception
    {
        public PostInvalidStateException()
        {

        }

        public PostInvalidStateException(string message)
            : base(message)
        {

        }
    }
}
