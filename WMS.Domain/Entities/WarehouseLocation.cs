using WMS.Domain.Exceptions;

namespace WMS.Domain.Entities
{
    public class WarehouseLocation
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }

        public WarehouseLocation(string code, string description = "")
        {
            Id = Guid.NewGuid();
            Code = code;
            Description = description;
        }

        public void UpdateDetails(string code, string description)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new WmsNullOrEmptyException(nameof(code));
            }

            Code = code;
            Description = description;
        }
    }
}