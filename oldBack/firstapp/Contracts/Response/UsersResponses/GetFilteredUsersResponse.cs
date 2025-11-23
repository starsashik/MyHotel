using firstapp.Contracts.Other;

namespace firstapp.Contracts.Response.UsersResponses;

public record GetFilteredUsersResponse(
    List<UserDto> Users
);