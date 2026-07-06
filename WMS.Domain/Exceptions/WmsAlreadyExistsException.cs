namespace WMS.Domain.Exceptions
{
    public class WmsAlreadyExistsException : WmsException
    {
        public WmsAlreadyExistsException(string resourceName, string propertyName, string value)
            : base("Resource Already Exists", $"{resourceName} with {propertyName} '{value}' already exists.", 409)
        {
        }
    }
}