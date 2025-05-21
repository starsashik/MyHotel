using System.Net.Http.Headers;
using firstapp.Abstractions.Services;
using firstapp.Contracts;
using firstapp.Contracts.Response;
using firstapp.Contracts.Response.UsersResponses;
using firstapp.Models.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;


namespace firstapp.Controllers;

[ApiController]
[Route("[controller]")]
public class MyController : ControllerBase
{
    private readonly ILogService _logService;
    private readonly IWebHostEnvironment _env;

    public MyController(ILogService logService, IWebHostEnvironment env)
    {
        _logService = logService;
        _env = env;
    }

    [HttpGet("[action]")]
    public IActionResult TestLogText([FromQuery] string x)
    {
        if (_logService.Write(x))
        {
            return Ok("Успешно");
        }
        return Ok("Ошибка");
    }

    [HttpGet("[action]")]
    public IActionResult GetLogFile()
    {
        var fullPath = Path.Combine(_env.WebRootPath, "TempFile/123.txt");
        return File(System.IO.File.ReadAllBytes(fullPath), "text/plain");
    }
    
    [HttpGet("[action]/{filePath=}")]
    public IActionResult GetFile(string filePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, filePath);
        
        if (!System.IO.File.Exists(fullPath))
        {
            throw new FileNotFoundException($"File not found: {fullPath}");
        }

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fullPath, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return File(fullPath, contentType);
        
    }
    
    /*
    [HttpPost("[action]/{key=}/{value=}")]
    public IActionResult SetSessionData(string key, string value)
    {
        HttpContext.Session.SetString(key, value);
        return Ok($"Значение {key} сохранено в сессии");
    }
    
    [HttpGet("[action]{key}")]
    public IActionResult GetSessionData(string key)
    {
        var value = HttpContext.Session.GetString(key);
        if (string.IsNullOrEmpty(value))
        {
            return NotFound($"Значение для ключа {key} не найдено");
        }
        return Ok(new { Key = key, Value = value });
    }
    
    [HttpPost("[action]/{key}")]
    public IActionResult RemoveSessionData(string key)
    {
        HttpContext.Session.Remove(key);
        return Ok($"Значение {key} удалено из сессии");
    }
    */
}