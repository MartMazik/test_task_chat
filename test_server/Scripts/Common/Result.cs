namespace TestServer.Scripts.Common;

public class Result<T>
{
    public bool Success { get; }
    public string Message { get; }
    public T? Data { get; }

    public Result(T? data = default)
    {
        Success = true;
        Message = string.Empty;
        Data = data;
    }

    public Result(string message)
    {
        Success = false;
        Message = message;
        Data = default(T);
    }

    public bool IsError => !Success;
    public bool IsSuccess => Success;

    public override string ToString()
    {
        return $"Result{{Success: {Success}, Message: {Message}, Data: {Data}}}";
    }
}