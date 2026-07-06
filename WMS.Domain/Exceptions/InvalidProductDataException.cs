namespace WMS.Domain.Exceptions
{
    public class InvalidProductDataException : WmsException
    {
        public InvalidProductDataException(string message) : base(message, 400)
        {
        }
    }
}