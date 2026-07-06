namespace WMS.Domain.Exceptions
{
    public class WmsNullOrEmptyException : WmsException
    {
        public WmsNullOrEmptyException(string propertyName)
            : base("Validation Error", $"{propertyName} cannot be null or empty.", 400)
        {
        }
    }
}