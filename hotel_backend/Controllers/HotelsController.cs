using hotel_backend.Abstractions.Services;
using hotel_backend.Contracts;
using hotel_backend.Contracts.Other;
using hotel_backend.Contracts.Requests.HotelsRequests;
using hotel_backend.Contracts.Response.HotelsResponses;
using hotel_backend.Exceptions.SpecificExceptions;
using hotel_backend.Models.Filters;
using hotel_backend.Models.Others;
using hotel_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_backend.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class HotelsController : ControllerBase
{
    private readonly IAccessCheckService _accessCheckService;
    private readonly IHotelsService _hotelsService;
    private readonly IWebHostEnvironment _environment;

    public HotelsController(IAccessCheckService accessCheckService, IHotelsService hotelsService, IWebHostEnvironment environment)
    {
        _accessCheckService = accessCheckService;
        _hotelsService = hotelsService;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> TestGetHotels(CancellationToken cancellationToken)
    {
        HotelFilter? hotelFilter = null;

        var hotels = await _hotelsService
            .GetFilteredHotelsAsync(hotelFilter, cancellationToken);

        var response = new List<TestHotels>(
            hotels
                .Select(u => new TestHotels(
                    u.Id.ToString(),
                    u.Name,
                    u.Location,
                    u.Description,
                    u.ImgUrl))
                .ToList());

        return Ok(response);
    }


    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> NOTUSEGetHotel([FromQuery] GetHotelRequest request,
        CancellationToken cancellationToken)
    {
        var hotel = await _hotelsService
            .GetHotelAsync(request.HotelId, cancellationToken);

        var response = new BaseResponse<GetHotelResponse>(
            new GetHotelResponse(
                new HotelDto(
                    hotel.Id,
                    hotel.Name,
                    hotel.Location,
                    hotel.Description,
                    hotel.ImgUrl)),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> NOTUSEGetFilteredHotels([FromQuery] GetFilteredHotelsRequest request,
        CancellationToken cancellationToken)
    {
        await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        var (hotelFilter, userHotelError) = HotelFilter
            .Create(request.PartOfName,
                request.PartOfLocation,
                request.PartOfDescription);

        if (!string.IsNullOrEmpty(userHotelError))
            throw new ConversionException($"Incorrect data format: {userHotelError}");

        var filteredHotels = await _hotelsService
            .GetFilteredHotelsAsync(hotelFilter, cancellationToken);

        var response = new BaseResponse<GetFilteredHotelsResponse>(
            new GetFilteredHotelsResponse(
                filteredHotels
                    .Select(hotel =>
                        new HotelDto(
                            hotel.Id,
                            hotel.Name,
                            hotel.Location,
                            hotel.Description,
                            hotel.ImgUrl))
                    .ToList()),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromForm] CreateHotelRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        string imageUrl = "ImgHotel/default.png"; // Дефолтное значение

        try
        {
            if (request.ImageFile != null)
            {
                // Генерируем уникальное имя файла
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ImageFile.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "ImgHotel", fileName);

                // Сохраняем файл
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream, cancellationToken);
                }

                imageUrl = $"ImgHotel/{fileName}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        var (hotel, hotelError) = Models.Hotel
            .Create(Guid.NewGuid(),
                request.Name,
                request.Location,
                request.Description,
                imageUrl);

        if (!string.IsNullOrEmpty(hotelError))
            throw new ConversionException($"Incorrect data format: {hotelError}");

        var createdHotelId = await _hotelsService
            .CreateHotelAsync(hotel, cancellationToken);

        var response = new BaseResponse<CreateHotelResponse>(
            new CreateHotelResponse(
                createdHotelId),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateHotel([FromForm] UpdateHotelRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        string imageUrl = "ImgHotel/default.png"; // Дефолтное значение

        try
        {
            if (request.ImageFile != null)
            {
                // Генерируем уникальное имя файла
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ImageFile.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "ImgHotel", fileName);

                // Сохраняем файл
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream, cancellationToken);
                }

                imageUrl = $"ImgHotel/{fileName}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        var oldHotel = await _hotelsService
            .GetHotelAsync(request.HotelId, cancellationToken);

        string newName = oldHotel.Name;
        if (request.NewName != null)
        {
            newName = request.NewName;
        }

        string NewLocation = oldHotel.Location;
        if (request.NewLocation != null)
        {
            NewLocation = request.NewLocation;
        }

        string NewDescription = oldHotel.Description;
        if (request.NewDescription != null)
        {
            NewDescription = request.NewDescription;
        }

        var (newHotel, newHotelError) = Models.Hotel
            .Create(request.HotelId,
                newName,
                NewLocation,
                NewDescription,
                imageUrl
                );

        if (!string.IsNullOrEmpty(newHotelError))
            throw new ConversionException($"Incorrect data format: {newHotelError}");


        var updatedHotelId = await _hotelsService
            .UpdateHotelAsync(request.HotelId, newHotel, cancellationToken);

        var response = new BaseResponse<UpdateHotelResponse>(
            new UpdateHotelResponse(
                updatedHotelId),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteHotel([FromQuery] DeleteHotelRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.AdministratorMin,
            cancellationToken);

        var deletedHotelId = await _hotelsService
            .DeleteHotelAsync(request.HotelId, cancellationToken);

        var response = new BaseResponse<DeleteHotelRequest>(
            new DeleteHotelRequest(
                deletedHotelId),
            null);

        return Ok(response);
    }
}