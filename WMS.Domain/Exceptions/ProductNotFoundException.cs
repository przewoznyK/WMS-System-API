namespace WMS.Domain.Exceptions
{
    public class ProductNotFoundException : WmsException
    {
        public ProductNotFoundException(string message) : base(message, 404)
        {
        }
    }
}
