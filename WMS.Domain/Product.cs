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
            Id = Guid.NewGuid();
            Sku = sku;
            Name = name;
            Description = description;
        }
    }
}