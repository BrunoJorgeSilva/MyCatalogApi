namespace MyCatalogApi.Application.ResultPattern
{
    public class Result<T>
    {
        public T? Value { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }

        public Result(T ValueReturn, string Message, bool Succes)
        {
            Value = ValueReturn;
            ErrorMessage = Message;
            IsSuccess = Succes;
        }


        private Result(T value)
        {
            Value = value;
            IsSuccess = true;
        }

        private Result(string errorMessage, T empty)
        {
            ErrorMessage = errorMessage;
            IsSuccess = false;
            Value = empty;
        }

        public static Result<T> Success(T value) => new Result<T>(value);

        public static Result<T> Failure(string errorMessage, T empty) => new Result<T>(errorMessage, empty);
    }
}
