using firstapp.Contracts.Other;

namespace firstapp.Contracts.Response.UsersResponses;

public record GetUserResponse(
    UserDto User
);