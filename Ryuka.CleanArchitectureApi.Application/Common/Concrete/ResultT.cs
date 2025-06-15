namespace Ryuka.NlayerApi.Application.Common.Concrete;

public class Result<T> : Result where T : class
{
    private readonly T _value;
    public T Data
    {
        get
        {
            return _value;
        }
    }

    public Result(T value,bool isSuccsess, List<string>? errors = null, string? message = null) 
        : base(isSuccsess, errors, message)
    {
        if (isSuccsess && value == null)
        {
            throw new InvalidOperationException("The result is not succssess");
        }
        _value = value;
    }

    public static Result<T> Success(T value, string? message = null)
    {
        return new Result<T>(value, true, null, message);
    }

    public static Result<T> Failure(List<string> errrors,string? message = null)
    {
        return new Result<T>(default(T)!, false, errrors, message);
    }
}