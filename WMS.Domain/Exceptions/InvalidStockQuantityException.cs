namespace WMS.Domain.Exceptions
{
    public class InvalidStockQuantityException : WmsException
    {
        public InvalidStockQuantityException(string message) : base(message, 400)
        {
        }
    }
}