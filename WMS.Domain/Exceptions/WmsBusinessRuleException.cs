namespace WMS.Domain.Exceptions
{
    public class WmsBusinessRuleException : WmsException
    {
        public WmsBusinessRuleException(string message)
            : base("Business Rule Violation", message, 400)
        {
        }
    }
}