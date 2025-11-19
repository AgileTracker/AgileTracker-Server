namespace agileTrackerServer.Models.ViewModels;

public class ResultViewModel
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public object? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public ResultViewModel() { }

    public ResultViewModel(
        string message,
        bool success = true,
        object? data = null,
        List<string>? errors = null)
    {
        Message = message;
        Success = success;
        Data = data;
        Errors = errors ?? new();
    }

    public static ResultViewModel Ok(string message, object? data = null)
        => new(message, true, data);

    public static ResultViewModel Fail(string message, List<string>? errors = null)
        => new(message, false, null, errors ?? new());
}
