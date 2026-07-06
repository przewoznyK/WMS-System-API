namespace WMS.Domain.Exceptions
{
    public abstract class WmsException : Exception
    {
        public string Title { get; }
        public int StatusCode { get; }

        protected WmsException(string title, string message, int statusCode) : base(message)
        {
            Title = title;
            StatusCode = statusCode;
        }
    }
}