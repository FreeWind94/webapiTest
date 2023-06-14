namespace webapiTest.Models;

public readonly struct ServiceResponse<T>
{
    public ServiceResponse(T value) {
        Succes = true;
        Data = value;
        Message = string.Empty;
    }

    public ServiceResponse(Exception e) {
        Succes = false;
        Message = e.Message;
    }


    public T? Data { get; }
    public bool Succes { get; }
    public string Message { get; }

    public static implicit operator ServiceResponse<T>(T value) => new(value);
    public static implicit operator ServiceResponse<T>(Exception e) => new(e);

    public R Match<R>(Func<ServiceResponse<T>, R> Succ, Func<ServiceResponse<T>, R> Fail) => 
        Succes ? Succ(this) : Fail(this);
}