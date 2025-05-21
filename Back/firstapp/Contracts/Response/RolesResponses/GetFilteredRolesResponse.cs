using firstapp.Contracts.Other;

namespace firstapp.Contracts.Response.RolesResponses;

public record GetFilteredRolesResponse(
    List<RoleDto> Roles
);