using hotel_backend.Contracts.Other;

namespace hotel_backend.Contracts.Response.RolesResponses;

public record GetFilteredRolesResponse(
    List<RoleDto> Roles
);