using System.Text;
using hotel_backend.Abstractions.Services;

namespace hotel_backend.Services;

public class SimpleLogService(IWebHostEnvironment env) : ILogService
{
    public bool Write(string message)
    {
        try
        {
            Console.WriteLine(message);
            var fullPath = Path.Combine(env.WebRootPath, "TempFile/123.txt");
            using (FileStream fstream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes(message);
                // запись массива байтов в файл
                fstream.Write(buffer, 0, buffer.Length);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка записи в файл");
            return false;
        }
        Console.WriteLine("Текст записан в файл");
        return true;
    }
}