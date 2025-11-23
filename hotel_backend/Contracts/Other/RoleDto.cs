namespace hotel_backend.Contracts.Other;

public record RoleDto(
    Guid Id,
    string Name,
    int AccessLevel
);