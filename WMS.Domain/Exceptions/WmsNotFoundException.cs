namespace WMS.Domain.Exceptions
{
    public class WmsNotFoundException : WmsException
    {
        public WmsNotFoundException(string resourceName, object key)
            : base("Resource Not Found", $"{resourceName} with ID {key} was not found.", 404)
        {
        }

        public WmsNotFoundException(string resourceName, string identifierName, object identifierValue)
            : base("Resource Not Found", $"{resourceName} with {identifierName} '{identifierValue}' was not found.", 404)
        {
        }

        public WmsNotFoundException(string message)
            : base("Resource Not Found", message, 404)
        {
        }
    }
}