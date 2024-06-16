namespace MediaAPI.Models
{
    public class GqlResult<T>
    {
        public T? Data { get; set; }
        public string? Error { get; set; }

        public static GqlResult<T> Success(T data) => new GqlResult<T> { Data = data };
        public static GqlResult<T> Failure(string error) => new GqlResult<T> { Error = error };
    }
}
