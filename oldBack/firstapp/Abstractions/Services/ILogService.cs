namespace firstapp.Abstractions.Services;

public interface ILogService
{
    bool Write(string message);
}