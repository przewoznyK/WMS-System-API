namespace WMS.Domain.Exceptions
{
    public abstract class WmsException : Exception
    {
        public int StatusCode { get; }

        public WmsException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
