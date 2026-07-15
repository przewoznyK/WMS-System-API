using WMS.Client.Services.Interfaces;

namespace WMS.Client.Services.Implementations
{
    public class ThemeService : IThemeService
    {
        public bool IsDarkMode { get; private set; }

        public event Action? ThemeChanged;

        public void Toggle()
        {
            IsDarkMode = !IsDarkMode;
            ThemeChanged?.Invoke();
            Console.WriteLine($"ThemeService.Toggle -> {IsDarkMode}");
        }
    }
}