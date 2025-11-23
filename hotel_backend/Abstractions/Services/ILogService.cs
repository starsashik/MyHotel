namespace hotel_backend.Abstractions.Services;

public interface ILogService
{
    bool Write(string message);
}