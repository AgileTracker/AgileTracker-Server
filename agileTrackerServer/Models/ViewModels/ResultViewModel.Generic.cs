namespace agileTrackerServer.Models.ViewModels;

public class ResultViewModel<T> : ResultViewModel
{
    public new T? Data
    {
        get => (T?)base.Data;
        set => base.Data = value;
    }

    public ResultViewModel() { }

    public ResultViewModel(
        string message,
        bool success = true,
        T? data = default,
        List<string>? errors = null)
        : base(message, success, data, errors)
    {
    }

    public static ResultViewModel<T> Ok(string message, T data)
        => new(message, true, data);

    public static ResultViewModel<T> Fail(string message, List<string>? errors)
        => new(message, false, default, errors);
}