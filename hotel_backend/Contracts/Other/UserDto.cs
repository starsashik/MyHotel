namespace hotel_backend.Contracts.Other;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    string PasswordHash,
    RoleDto Role,
    string ImgUrl
);