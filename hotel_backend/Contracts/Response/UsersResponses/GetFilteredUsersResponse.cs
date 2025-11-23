using hotel_backend.Contracts.Other;

namespace hotel_backend.Contracts.Response.UsersResponses;

public record GetFilteredUsersResponse(
    List<UserDto> Users
);