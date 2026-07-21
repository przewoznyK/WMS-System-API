using System.Text.RegularExpressions;
using WMS.Domain.Exceptions;

namespace WMS.Domain.Entities
{
    public class WarehouseLocation
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        private const string CodePattern = @"^[A-Z]{2}-\d{2}-\d{2}$";

        private WarehouseLocation() 
        {
            Code = null!;
            Description = null!;
        }

        public WarehouseLocation(string code, string? description = "")
        {
            ValidateCode(code);

            Id = Guid.NewGuid();
            Code = code;
            Description = description ?? "";
        }

        public void UpdateDetails(string code, string? description)
        {
            ValidateCode(code);

            Code = code;
            Description = description ?? "";
        }

        private void ValidateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new WmsNullOrEmptyException(nameof(code));
            }

            if (!Regex.IsMatch(code, CodePattern))
            {
                throw new WmsBusinessRuleException("The format must comply with the pattern: AA-00-00.");
            }
        }
    }
}