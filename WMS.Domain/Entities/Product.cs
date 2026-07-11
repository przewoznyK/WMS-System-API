using WMS.Domain.Exceptions;

namespace WMS.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Sku { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        private Product() 
        {
            Sku = null!;
            Name = null!;
            Description = null!;
        }

        public Product(string sku, string name, string description = "")
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new WmsNullOrEmptyException(nameof(sku));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new WmsNullOrEmptyException(nameof(name));
            }

            Id = Guid.NewGuid();
            Sku = sku;
            Name = name;
            Description = description;
        }

        public void UpdateDetails(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new WmsNullOrEmptyException(nameof(name));
            }

            Name = name;
            Description = description;
        }
    }
}