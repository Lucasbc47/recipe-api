namespace Recipe.API.Models
{
    public class ResponseBase<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; } = null;
        public T? Data { get; set; }

        public static ResponseBase<T> CreateSuccess(T data) => new()
        {
            Success = true,
            Data = data
        };

        public static ResponseBase<T> CreateError(string message) => new()
        {
            Success = false,
            Message = message
        };
    }
    // Base Case for deletion
    public class ResponseBase
    {
        public bool Success { get; set; }
        public string? Message { get; set; } = null;

        public static ResponseBase CreateSuccess() => new() { Success = true };
        public static ResponseBase CreateError(string message) => new() { Success = false, Message = message };
    }
}