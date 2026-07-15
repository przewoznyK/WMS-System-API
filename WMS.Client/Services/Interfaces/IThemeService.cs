namespace WMS.Client.Services.Interfaces
{
    public interface IThemeService
    {
        bool IsDarkMode { get; }
        event Action? ThemeChanged;
        void Toggle();
    }
}