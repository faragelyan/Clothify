namespace Clothify.Application.DTOs
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }

        public static Result<T> Ok(T data) =>
            new Result<T> { Success = true, Data = data };

        public static Result<T> Fail(string error) =>
            new Result<T> { Success = false, Error = error };
    }
}
