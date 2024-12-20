namespace DanceCompetitionHelper.Exceptions
{
    public class NoDataFoundException : Exception
    {
        public object? RouteObjects { get; set; }

        public NoDataFoundException()
        {
        }

        public NoDataFoundException(
            string? message)
            : base(
                  message)
        {
        }

        public NoDataFoundException(
            string? message,
            Exception? innerException)
            : base(
                  message,
                  innerException)
        {
        }
    }
}
