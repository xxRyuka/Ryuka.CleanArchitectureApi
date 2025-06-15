namespace Ryuka.NlayerApi.Application.Common.Abstract;


public interface IResult
{
    
    // aslında kullanmasakta olur ilerleyen zamanda bakacağim buraya 
    bool isFailure { get; }
    bool isSuccess { get; }
    string? Message { get; }
    List<string>? Errors{ get; }
    
}