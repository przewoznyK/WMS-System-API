using MudBlazor;
using System.Text.RegularExpressions;

namespace WMS.Client.Services
{
    public class UiUtilityService
    {
        public IMask LocationMask = new PatternMask("XX-00-00")
        {
            MaskChars = new[]
            {
                new MaskChar('X', @"[A-Za-z]"),
                new MaskChar('0', @"[0-9]")
            },
            Transformation = c => char.ToUpperInvariant(c)
        };

        public string SplitPascalCase(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            return Regex.Replace(text, "([a-z])([A-Z])", "$1 $2");
        }

        public Task<IEnumerable<string>> FilterLocationCodes(IEnumerable<string>? locations, string value, string? excludeValue = null)
        {
            if (locations == null)
            {
                return Task.FromResult(Enumerable.Empty<string>());
            }

            var filtered = locations.AsEnumerable();

            if (!string.IsNullOrEmpty(excludeValue))
            {
                filtered = filtered.Where(x => !x.Equals(excludeValue, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(value))
            {
                filtered = filtered.Where(x => x.Contains(value, StringComparison.OrdinalIgnoreCase));
            }

            return Task.FromResult(filtered);
        }
    }
}
