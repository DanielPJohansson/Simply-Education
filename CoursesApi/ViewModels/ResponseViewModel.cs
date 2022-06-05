using System.Text.Json;

namespace CoursesApi.ViewModels
{
    public class ResponseViewModel
    {
        public int StatusCode { get; set; }
        public int Count { get; set; }
        public string Data { get; set; } = string.Empty;

        public ResponseViewModel(int statusCode, string data, int count = 1)
        {
            StatusCode = statusCode;
            Count = count;
            Data = data;
        }
    }
}