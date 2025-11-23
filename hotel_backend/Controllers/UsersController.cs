using hotel_backend.Abstractions.Services;
using hotel_backend.Contracts;
using hotel_backend.Contracts.Other;
using hotel_backend.Contracts.Requests;
using hotel_backend.Contracts.Requests.UsersRequests;
using hotel_backend.Contracts.Response;
using hotel_backend.Contracts.Response.UsersResponses;
using hotel_backend.Exceptions.SpecificExceptions;
using hotel_backend.Models.Filters;
using hotel_backend.Models.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = hotel_backend.Abstractions.Services.IAuthorizationService;

namespace hotel_backend.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UsersController : ControllerBase
{
    private readonly IAccessCheckService _accessCheckService;
    private readonly IRolesService _rolesService;
    private readonly IUsersService _usersService;
    private readonly IWebHostEnvironment _environment;
    private readonly IAuthorizationService _authorizationService;

    public UsersController(IAccessCheckService accessCheckService, IUsersService usersService,
        IRolesService rolesService,IWebHostEnvironment environment, IAuthorizationService authorizationService)
    {
        _accessCheckService = accessCheckService;
        _rolesService = rolesService;
        _usersService = usersService;
        _environment = environment;
        _authorizationService = authorizationService;
    }

    [HttpGet]
    public async Task<IActionResult> TestGetUsers(CancellationToken cancellationToken)
    {
        UserFilter? userFilter = null;

        var users = await _usersService
            .GetFilteredUsersAsync(userFilter, cancellationToken);

        var response = new List<TestUsers>(
            users
                .Select(u => new TestUsers(
                    u.Id.ToString(),
                    u.Email,
                    u.Name,
                    u.Role.Id.ToString(),
                    u.ImgUrl))
                .ToList());

        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> NOTUSEGetUser([FromQuery] GetUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _usersService
            .GetUserAsync(request.UserId, cancellationToken);

        var response = new BaseResponse<GetUserResponse>(
            new GetUserResponse(
                new UserDto(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.PasswordHash,
                    new RoleDto(
                        user.Role.Id,
                        user.Role.Name,
                        user.Role.AccessLevel),
                    user.ImgUrl)),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> NOTUSEGetFilteredUsers([FromQuery] GetFilteredUsersRequest request,
        CancellationToken cancellationToken)
    {
        await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        var (userFilter, userFilterError) = UserFilter
            .Create(request.PartOfName,
                request.PartOfEmail,
                request.Role);

        if (!string.IsNullOrEmpty(userFilterError))
            throw new ConversionException($"Incorrect data format: {userFilterError}");

        var filteredUsers = await _usersService
            .GetFilteredUsersAsync(userFilter, cancellationToken);

        var response = new BaseResponse<GetFilteredUsersResponse>(
            new GetFilteredUsersResponse(
                filteredUsers
                    .Select(user =>
                        new UserDto(
                            user.Id,
                            user.Name,
                            user.Email,
                            user.PasswordHash,
                            new RoleDto(
                                user.Role.Id,
                                user.Role.Name,
                                user.Role.AccessLevel),
                            user.ImgUrl))
                    .ToList()),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        var role = await _rolesService.GetRoleAsync(request.Role, cancellationToken);

        var (user, userError) = Models.User
            .Create(Guid.NewGuid(),
                request.Name,
                request.Email,
                request.Password,
                false,
                role);

        if (!string.IsNullOrEmpty(userError))
            throw new ConversionException($"Incorrect data format: {userError}");

        if (u.Role.AccessLevel >= user.Role.AccessLevel)
            throw new AccessException("The user does not have sufficient access rights");

        var createdUserId = await _usersService
            .CreateUserAsync(user, cancellationToken);

        var response = new BaseResponse<CreateUserResponse>(
            new CreateUserResponse(
                createdUserId),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> NOTUSEUpdateUser([FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        var newRole = await _rolesService.GetRoleAsync(request.NewRoleId, cancellationToken);

        var (newUser, newUserError) = Models.User
            .Create(request.UserId,
                request.NewName,
                request.NewEmail,
                request.NewPassword,
                false,
                newRole);

        if (!string.IsNullOrEmpty(newUserError))
            throw new ConversionException($"Incorrect data format: {newUserError}");

        if (u.Role.AccessLevel >= newUser.Role.AccessLevel)
            throw new AccessException("The user does not have sufficient access rights");

        var existingUser = await _usersService
            .GetUserAsync(request.UserId, cancellationToken);

        if (u.Role.AccessLevel >= existingUser.Role.AccessLevel)
            throw new AccessException("The user does not have sufficient access rights");

        var updatedUserId = await _usersService
            .UpdateUserAsync(request.UserId, newUser, cancellationToken);

        var response = new BaseResponse<UpdateUserResponse>(
            new UpdateUserResponse(
                updatedUserId),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateUserSmallParam([FromForm] UpdateUserRequestSmallParam request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.CommonUser,
            cancellationToken);

        var (userFilter, userFilterError) = UserFilter
            .Create(null,
                request.UserEmail,
                null);

        if (!string.IsNullOrEmpty(userFilterError))
            throw new ConversionException($"Incorrect data format: {userFilterError}");

        var filteredUsers = await _usersService
            .GetFilteredUsersAsync(userFilter, cancellationToken);

        string newName = filteredUsers[0].Name;
        if (request.NewName != null)
        {
            newName = request.NewName;
        }

        string imageUrl = "ImgProfile/default.png"; // Дефолтное значение
        try
        {
            if (request.ImageFile != null)
            {
                // Генерируем уникальное имя файла
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ImageFile.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "ImgProfile", fileName);

                // Сохраняем файл
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream, cancellationToken);
                }

                imageUrl = $"ImgProfile/{fileName}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        var (newUser, newUserError) = Models.User
            .Create(filteredUsers[0].Id,
                newName,
                filteredUsers[0].Email,
                filteredUsers[0].PasswordHash,
                true,
                filteredUsers[0].Role,
                imageUrl);

        if (!string.IsNullOrEmpty(newUserError))
            throw new ConversionException($"Incorrect data format: {newUserError}");

        await _usersService
            .UpdateUserAsync(filteredUsers[0].Id, newUser, cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromQuery] DeleteUserRequest request,
        CancellationToken cancellationToken)
    {
        var u = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.AdministratorMin,
            cancellationToken);

        var existingUser = await _usersService
            .GetUserAsync(request.UserId, cancellationToken);

        if (u.Role.AccessLevel >= existingUser.Role.AccessLevel)
            throw new AccessException("The user does not have sufficient access rights");

        var deletedUserId = await _usersService
            .DeleteUserAsync(request.UserId, cancellationToken);

        var response = new BaseResponse<DeleteUserResponse>(
            new DeleteUserResponse(
                deletedUserId),
            null);

        return Ok(response);
    }
}