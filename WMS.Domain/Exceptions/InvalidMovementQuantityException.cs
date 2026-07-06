namespace WMS.Domain.Exceptions
{
    public class InvalidMovementQuantityException : WmsException
    {
        public InvalidMovementQuantityException(string message) : base(message, 400)
        {
        }
    }
}