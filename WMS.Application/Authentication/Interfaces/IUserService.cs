namespace WMS.Application.Authentication.Interfaces
{
    public interface IUserService
    {
        Task<Dictionary<string, string>> GetUsersAsync(IEnumerable<string> userIds);
    }
}