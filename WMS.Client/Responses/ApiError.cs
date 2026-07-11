namespace WMS.Client.Responses
{
    public class ApiError
    {
        public string? Title { get; set; }
        public string? Details { get; set; }
        public Dictionary<string, string[]> Errors { get; set; } = [];
    }
}