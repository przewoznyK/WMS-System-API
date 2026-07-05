namespace WMS.Domain
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Sku { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Product(string sku, string name, string description = "")
        {
            if(string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("SKU cannot be null", nameof(sku));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null", nameof(name));
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
                throw new ArgumentException("Name cannot be null", nameof(name));
            }

            Name = name;
            Description = description;
        }
    }
}