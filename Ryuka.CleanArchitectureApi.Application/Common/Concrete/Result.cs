using Ryuka.NlayerApi.Application.Common.Abstract;

namespace Ryuka.NlayerApi.Application.Common.Concrete;

public class Result : IResult
{
    public bool isFailure => !isSuccess;
    public bool isSuccess { get; private set; }
    public string? Message { get; private set; }
    public List<string>? Errors { get; private set; } 

    public Result(bool _isSuccsess,List<string>? errors = null ,string? message = null)
    {
        this.isSuccess = _isSuccsess;
        this.Errors = errors ?? new List<string>();
        this.Message = message;
    }
    
    public static Result Succses(string? message = null)
    {
        return new Result(true, null, message);
    }

    public static Result Fail(List<string> errors,string? message=null)
    {
        return new Result(false, errors, message);
    }

    
}