﻿namespace firstapp.Contracts.Requests.RolesRequests;

public record UpdateRoleRequest(
    Guid RoleId,
    string NewName,
    int NewAccessLevel
);