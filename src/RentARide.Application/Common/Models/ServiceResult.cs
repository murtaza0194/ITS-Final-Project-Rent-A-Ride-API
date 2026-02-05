namespace RentARide.Application.Common.Models;

public class ServiceResult<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; } // e.g. 200, 201, 400, 404

    public static ServiceResult<T> Ok(T data, string message = "Success", int statusCode = 200)
    {
        return new ServiceResult<T> { Data = data, Success = true, Message = message, StatusCode = statusCode };
    }

    public static ServiceResult<T> Failure(string message, int statusCode = 400)
    {
        return new ServiceResult<T> { Success = false, Message = message, StatusCode = statusCode };
    }
}

public class ServiceResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }

    public static ServiceResult Ok(string message = "Success", int statusCode = 200)
    {
        return new ServiceResult { Success = true, Message = message, StatusCode = statusCode };
    }

    public static ServiceResult Failure(string message, int statusCode = 400)
    {
        return new ServiceResult { Success = false, Message = message, StatusCode = statusCode };
    }
}
