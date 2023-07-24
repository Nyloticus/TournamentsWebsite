namespace MVC.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public string Errors_String { get; set; }
        public int Code { get; set; }
        public List<T> Payload { get; set; }
        public string Message { get; set; }

    }

}
