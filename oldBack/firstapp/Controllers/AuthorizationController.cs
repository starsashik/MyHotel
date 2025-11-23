using System.Diagnostics;
using firstapp.Abstractions.Services;
using firstapp.Contracts;
using firstapp.Contracts.Requests;
using firstapp.Contracts.Requests.AuthorizationRequests;
using firstapp.Contracts.Response;
using firstapp.Contracts.Response.AuthorizationResponses;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models.Filters;
using firstapp.Models.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = firstapp.Abstractions.Services.IAuthorizationService;

namespace firstapp.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthorizationController : ControllerBase
{
    private readonly IAccessCheckService _accessCheckService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUsersService _usersService;

    public AuthorizationController(IAccessCheckService accessCheckService, IAuthorizationService authorizationService, IUsersService usersService)
    {
        _accessCheckService = accessCheckService;
        _authorizationService = authorizationService;
        _usersService = usersService;
    }
    
    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest loginUserRequest,
        CancellationToken cancellationToken)
    {
        var token = await _authorizationService
            .LoginUser(loginUserRequest.Email, loginUserRequest.Password, cancellationToken);

        Response.Cookies.Append("jwt_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.Now.AddHours(1)
        });

        var response = new BaseResponse<LoginUserResponse>(
            new LoginUserResponse(
                token),
            null);

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest registerUserRequest,
        CancellationToken cancellationToken)
    {
        var token = await _authorizationService.RegisterUser(registerUserRequest.Name, registerUserRequest.Email,
            registerUserRequest.Password, cancellationToken);

        Response.Cookies.Append("jwt_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.Now.AddHours(1)
        });

        var response = new BaseResponse<RegisterUserResponse>(
            new RegisterUserResponse(
                token),
            null);

        return Ok(response);
    }


    [HttpGet]
    public Task<IActionResult> NOTUSEGetCurrentUserToken(CancellationToken cancellationToken)
    {
        Request.Cookies.TryGetValue("jwt_token", out var token);

        var response = new BaseResponse<GetCurrentUserTokenResponse>(
            new GetCurrentUserTokenResponse(
                token ?? string.Empty),
            null);

        return Task.FromResult<IActionResult>(Ok(response));
    }

    

    [HttpPost]
    public async Task<IActionResult> NOTUSEValidateToken([FromBody] ValidateTokenRequest? validateTokenRequest,
        CancellationToken cancellationToken)
    {
        Request.Cookies.TryGetValue("jwt_token", out var token);

        if (token is null)
            throw new AuthorizationException("Token is somehow null, but it is impossible");

        if (validateTokenRequest is not null)
        {
            await _accessCheckService.CheckAccessLevel(
                HttpContext,
                (int)AccessLevelEnumerator.CommonUser,
                cancellationToken);

            token = validateTokenRequest.Token;
        }

        var tokenData = await _authorizationService.ValidateToken(token, cancellationToken);

        var response = new BaseResponse<ValidateTokenResponse>(
            new ValidateTokenResponse(
                token,
                tokenData),
            null);

        return Ok(response);
    }


    [HttpPost]
    public Task<IActionResult> LogoutUser()
    {
        Response.Cookies.Append("jwt_token", String.Empty, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.Now.AddHours(1)
        });

        return Task.FromResult<IActionResult>(Ok(""));
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentDataUser(CancellationToken cancellationToken)
    {
        Request.Cookies.TryGetValue("jwt_token", out var token);

        token = token ?? string.Empty;

        if (token != String.Empty)
        {
            var tokenData = await _authorizationService.ValidateToken(token, cancellationToken);

            int asseccLvl = 0;
            switch (tokenData.RoleGroup)
            {
                case "SuperUser":
                {
                    asseccLvl = 2;
                    break;
                }
                case "Administrator":
                {
                    asseccLvl = 1;
                    break;
                }
                case "Editor":
                {
                    asseccLvl = 1;
                    break;
                }
                case "CommonUser":
                {
                    asseccLvl = 0;
                    break;
                }
            }

            var (userFilter, userFilterError) = UserFilter
                .Create(null,
                    tokenData.Email,
                    null);

            var filteredUsers = await _usersService
                .GetFilteredUsersAsync(userFilter, cancellationToken);

            var response = new GetCurrentUserDataResponse(
                token,
                filteredUsers[0].Name,
                tokenData.Email,
                asseccLvl,
                filteredUsers[0].ImgUrl,
                filteredUsers[0].Id.ToString()
            );

            return Ok(response);
        }
        else
        {
            var response = new GetCurrentUserDataResponse(
                "",
                "",
                "",
                -1,
                "",
                ""
            );

            return Ok(response);
        }
    }

    [HttpPost]
    public Task<IActionResult> SetEmptyCookies()
    {
        Response.Cookies.Append("jwt_token", String.Empty, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.Now.AddHours(1)
        });
        
        return Task.FromResult<IActionResult>(Ok(""));
    }
}
