using System.Globalization;
using firstapp.Abstractions.Repositories;
using firstapp.Abstractions.Services;
using firstapp.Contracts;
using firstapp.Contracts.Other;
using firstapp.Contracts.Requests.BookingsRequests;
using firstapp.Contracts.Response.BookingsResponces;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models.Filters;
using firstapp.Models.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace firstapp.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class BookingsController : ControllerBase
{
    private readonly IAccessCheckService _accessCheckService;
    private readonly IBookingService _bookingService;

    public BookingsController(IAccessCheckService accessCheckService, IBookingService bookingService)
    {
        _accessCheckService = accessCheckService;
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> TestGetBookings(CancellationToken cancellationToken)
    {
        BookingFilter? bookingFilter = null;

        var bookings = await _bookingService
            .GetFilteredBookingsAsync(bookingFilter, cancellationToken);

        var response = new List<TestBookings>(
            bookings
                .Select(u => new TestBookings(
                    u.Id.ToString(),
                    u.UserId.ToString(),
                    u.RoomId.ToString(),
                    u.CheckInDate,
                    u.CheckOutDate))
                .ToList());

        return Ok(response);
    }

    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> NOTUSEGetBooking([FromQuery] GetBookingRequest request,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingService
            .GetBookingAsync(request.BookingId, cancellationToken);

        var response = new BaseResponse<GetBookingResponse>(
            new GetBookingResponse(
                new BookingDto(
                    booking.Id.ToString(),
                    booking.UserId.ToString(),
                    booking.RoomId.ToString(),
                    booking.CheckInDate,
                    booking.CheckOutDate)),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> GetFilteredBookings([FromForm] GetFilteredBookingsRequest request,
        CancellationToken cancellationToken)
    {
        await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.CommonUser,
            cancellationToken);

        var (bookingFilter, bookingError) = BookingFilter
            .Create(request.UserId,
                request.DateInRange is null ? null : DateOnly.ParseExact(request.DateInRange, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                );

        if (!string.IsNullOrEmpty(bookingError))
            throw new ConversionException($"Incorrect data format: {bookingError}");

        var filteredBookings = await _bookingService
            .GetFilteredBookingsAsync(bookingFilter, cancellationToken);

        /*
        var response = new BaseResponse<GetFilteredBookingsResponse>(
            new GetFilteredBookingsResponse(
                filteredBookings
                    .Select(booking =>
                        new BookingDto(
                            booking.Id.ToString(),
                            booking.UserId.ToString(),
                            booking.RoomId.ToString(),
                            booking.CheckInDate,
                            booking.CheckOutDate))
                    .ToList()),
            null);
        return Ok(response);
        */

        var response = new List<TestBookings>(
            filteredBookings
                .Select(u => new TestBookings(
                    u.Id.ToString(),
                    u.UserId.ToString(),
                    u.RoomId.ToString(),
                    u.CheckInDate,
                    u.CheckOutDate))
                .ToList());

        return Ok(response);

    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromForm] CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.CommonUser,
            cancellationToken);

        var (booking, bookingError) = Models.Booking
            .Create(Guid.NewGuid(),
                request.UserId,
                request.RoomId,
                DateOnly.ParseExact(request.CheckIn, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                DateOnly.ParseExact(request.CheckOut, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                );

        if (!string.IsNullOrEmpty(bookingError))
            throw new ConversionException($"Incorrect data format: {bookingError}");

        var createdBookinglId = await _bookingService
            .CreateBookingAsync(booking, cancellationToken);

        var response = new BaseResponse<CreateBookingResponse>(
            new CreateBookingResponse(
                createdBookinglId),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> NOTUSEUpdateBooking([FromForm] UpdateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.CommonUser,
            cancellationToken);

        var oldBooking = await _bookingService
            .GetBookingAsync(request.Id, cancellationToken);

        var (newBooking, bookingError) = Models.Booking
            .Create(request.Id,
                request.UserId ?? oldBooking.UserId,
                request.RoomId ?? oldBooking.RoomId,
                request.CheckInDate is null ? oldBooking.CheckInDate : DateOnly.ParseExact(request.CheckInDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                request.CheckOutDate is null ? oldBooking.CheckOutDate : DateOnly.ParseExact(request.CheckOutDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                );

        if (!string.IsNullOrEmpty(bookingError))
            throw new ConversionException($"Incorrect data format: {bookingError}");

        var updatedBookinglId = await _bookingService
            .UpdateBookingAsync(request.Id, newBooking, cancellationToken);

        var response = new BaseResponse<UpdateBookingResponse>(
            new UpdateBookingResponse(
                updatedBookinglId),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteBooking([FromQuery] DeleteBookingRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.CommonUser,
            cancellationToken);

        var deletedBookingId = await _bookingService
            .DeleteBookingAsync(request.BookingId, cancellationToken);

        var response = new BaseResponse<DeleteBookingResponse>(
            new DeleteBookingResponse(
                deletedBookingId),
            null);

        return Ok(response);
    }
}