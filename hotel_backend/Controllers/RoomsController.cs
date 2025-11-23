using hotel_backend.Abstractions.Services;
using hotel_backend.Contracts;
using hotel_backend.Contracts.Other;
using hotel_backend.Contracts.Requests.RoomsRequests;
using hotel_backend.Contracts.Response.HotelsResponses;
using hotel_backend.Contracts.Response.RoomsResponse;
using hotel_backend.Contracts.Response.UsersResponses;
using hotel_backend.Exceptions.SpecificExceptions;
using hotel_backend.Models.Filters;
using hotel_backend.Models.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_backend.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class RoomsController : ControllerBase
{
    private readonly IAccessCheckService _accessCheckService;
    private readonly IRoomsService _roomsService;
    private readonly IWebHostEnvironment _environment;

    public RoomsController(IAccessCheckService accessCheckService, IRoomsService roomsService,IWebHostEnvironment environment)
    {
        _accessCheckService = accessCheckService;
        _roomsService = roomsService;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> TestGetRooms(CancellationToken cancellationToken)
    {
        RoomFilter? roomFilter = null;

        var rooms = await _roomsService
            .GetFilteredRoomsAsync(roomFilter, cancellationToken);

        var response = new List<TestRooms>(
            rooms
                .Select(u => new TestRooms(
                    u.Id.ToString(),
                    u.RoomNumber,
                    u.RoomType,
                    u.PricePerNight,
                    u.ImgUrl,
                    u.HotelId.ToString()))
                .ToList());

        return Ok(response);
    }

    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> NOTUSEGetRoom([FromBody] GetRoomRequest request,
        CancellationToken cancellationToken)
    {
        var room = await _roomsService
            .GetRoomAsync(request.RoomId, cancellationToken);

        var response = new BaseResponse<GetRoomResponse>(
            new GetRoomResponse(
                new RoomDto(
                    room.Id,
                    room.RoomNumber,
                    room.RoomType,
                    room.PricePerNight,
                    room.ImgUrl)),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> GetFilteredRooms([FromForm] GetFilteredRoomsRequest request,
        CancellationToken cancellationToken)
    {
        await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.CommonUser,
            cancellationToken);

        int? roomType = null;
        if (request.RoomType != null)
        {
            roomType = int.Parse(request.RoomType);
        }

        var (roomFilter, userHotelError) = RoomFilter
            .Create(request.HotelId,
                roomType);

        if (!string.IsNullOrEmpty(userHotelError))
            throw new ConversionException($"Incorrect data format: {userHotelError}");

        var filteredRooms = await _roomsService
            .GetFilteredRoomsAsync(roomFilter, cancellationToken);

        /*
        var response = new BaseResponse<GetFilteredRoomsResponse>(
            new GetFilteredRoomsResponse(
                filteredRooms
                    .Select(room =>
                        new RoomDto(
                            room.Id,
                            room.RoomNumber,
                            room.RoomType,
                            room.PricePerNight,
                            room.ImgUrl))
                    .ToList()),
            null);

        return Ok(response);
        */

        var response = new List<TestRooms>(
            filteredRooms
                .Select(u => new TestRooms(
                    u.Id.ToString(),
                    u.RoomNumber,
                    u.RoomType,
                    u.PricePerNight,
                    u.ImgUrl,
                    u.HotelId.ToString()))
                .ToList());

        return Ok(response);

    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromForm] CreateRoomRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        string imageUrl = "ImgRoom/default.png"; // Дефолтное значение

        try
        {
            if (request.ImageFile != null)
            {
                // Генерируем уникальное имя файла
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ImageFile.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "ImgRoom", fileName);

                // Сохраняем файл
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream, cancellationToken);
                }

                imageUrl = $"ImgRoom/{fileName}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        var (room, roomError) = Models.Room
            .Create(Guid.NewGuid(),
                request.HotelId,
                int.Parse(request.RoomNumber),
                int.Parse(request.RoomType),
                int.Parse(request.PricePerNight),
                imageUrl);

        if (!string.IsNullOrEmpty(roomError))
            throw new ConversionException($"Incorrect data format: {roomError}");

        var createdRoomlId = await _roomsService
            .CreateRoomAsync(room, cancellationToken);

        var response = new BaseResponse<CreateRoomResponse>(
            new CreateRoomResponse(
                createdRoomlId),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateRoom([FromForm] UpdateRoomRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        string imageUrl = "ImgRoom/default.png"; // Дефолтное значение

        try
        {
            if (request.ImageFile != null)
            {
                // Генерируем уникальное имя файла
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ImageFile.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "ImgRoom", fileName);

                // Сохраняем файл
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream, cancellationToken);
                }

                imageUrl = $"ImgRoom/{fileName}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        var oldRoom = await _roomsService
            .GetRoomAsync(request.Id, cancellationToken);

        int newRoomNumber = oldRoom.RoomNumber;
        if (request.NewRoomNumber != null)
        {
            newRoomNumber = int.Parse(request.NewRoomNumber);
        }

        int newRoomType = oldRoom.RoomType;
        if (request.NewRoomType != null)
        {
            newRoomType = int.Parse(request.NewRoomType);
        }

        int newPricePerNight = oldRoom.PricePerNight;
        if (request.NewPricePerNight != null)
        {
            newPricePerNight = int.Parse(request.NewPricePerNight);
        }


        var (newRoom, roomError) = Models.Room
            .Create(request.Id,
                oldRoom.HotelId,
                newRoomNumber,
                newRoomType,
                newPricePerNight,
                imageUrl
                );

        if (!string.IsNullOrEmpty(roomError))
            throw new ConversionException($"Incorrect data format: {roomError}");


        var updatedRoomlId = await _roomsService
            .UpdateRoomAsync(request.Id, newRoom, cancellationToken);

        var response = new BaseResponse<UpdateRoomResponse>(
            new UpdateRoomResponse(
                updatedRoomlId),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteRoom([FromQuery] DeleteRoomRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.AdministratorMin,
            cancellationToken);

        var deletedRoomId = await _roomsService
            .DeleteRoomAsync(request.RoomId, cancellationToken);

        var response = new BaseResponse<DeleteRoomResponse>(
            new DeleteRoomResponse(
                deletedRoomId),
            null);

        return Ok(response);
    }
}