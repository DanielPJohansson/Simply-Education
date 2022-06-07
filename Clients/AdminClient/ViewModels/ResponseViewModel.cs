using System.Text.Json;

namespace AdminClient.ViewModels
{
    public class ResponseViewModel
    {
        public int StatusCode { get; set; }
        public int Count { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}