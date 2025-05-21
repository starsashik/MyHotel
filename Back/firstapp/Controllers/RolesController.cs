﻿using firstapp.Abstractions.Services;
using firstapp.Contracts;
using firstapp.Contracts.Other;
using firstapp.Contracts.Requests.RolesRequests;
using firstapp.Contracts.Response.RolesResponses;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models;
using firstapp.Models.Filters;
using firstapp.Models.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace firstapp.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class RolesController : ControllerBase
{
    private readonly IAccessCheckService _accessCheckService;
    private readonly IRolesService _rolesService;

    public RolesController(IAccessCheckService accessCheckService, IRolesService rolesService)
    {
        _accessCheckService = accessCheckService;
        _rolesService = rolesService;
    }

    [HttpGet]
    public async Task<IActionResult> TestGetRoles(CancellationToken cancellationToken)
    {
        RoleFilter? roleFilter = null;

        var filteredRoles = await _rolesService
            .GetFilteredRolesAsync(roleFilter, cancellationToken);

        var response = new List<TestRoles>(
            filteredRoles
                .Select(role =>
                    {
                        var level = role.AccessLevel switch
                        {
                            7 => 0,
                            0 => 2,
                            _ => 1
                        };
                        return new TestRoles(
                            role.Id.ToString(),
                            role.Name,
                            level
                        );
                    }
                )
                .ToList());

        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> NOTUSEGetRole([FromQuery] GetRoleRequest request, CancellationToken cancellationToken)
    {
        var role = await _rolesService
            .GetRoleAsync(request.RoleId, cancellationToken);

        var response = new BaseResponse<GetRoleResponse>(
            new GetRoleResponse(
                new RoleDto(
                    role.Id,
                    role.Name,
                    role.AccessLevel)),
            null);

        return Ok(response);
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> NOTUSEGetFilteredRoles([FromQuery] GetFilteredRolesRequest request,
        CancellationToken cancellationToken)
    {
        await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.CommonUser,
            cancellationToken);

        var (roleFilter, roleFilterError) = RoleFilter
            .Create(request.PartOfName,
                request.AccessLevelFrom,
                request.AccessLevelTo);

        if (!string.IsNullOrEmpty(roleFilterError))
            throw new ConversionException($"Incorrect data format: {roleFilterError}");

        var filteredRoles = await _rolesService
            .GetFilteredRolesAsync(roleFilter, cancellationToken);

        var response = new BaseResponse<GetFilteredRolesResponse>(
            new GetFilteredRolesResponse(
                filteredRoles
                    .Select(role =>
                        new RoleDto(
                            role.Id,
                            role.Name,
                            role.AccessLevel))
                    .ToList()),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        var (role, roleError) = Role
            .Create(Guid.NewGuid(),
                request.Name,
                request.AccessLevel);

        if (!string.IsNullOrEmpty(roleError))
            throw new ConversionException($"Incorrect data format: {roleError}");

        if (user.Role.AccessLevel >= role.AccessLevel)
            throw new AccessException("The user does not have sufficient access rights");

        var createdRoleId = await _rolesService
            .CreateRoleAsync(role, cancellationToken);

        var response = new BaseResponse<CreateRoleResponse>(
            new CreateRoleResponse(
                createdRoleId),
            null);

        return Ok(response);
    }


    [Authorize]
    [HttpPut]
    public async Task<IActionResult> NOTUSEUpdateRole([FromBody] UpdateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        var (newRole, newRoleError) = Role
            .Create(request.RoleId,
                request.NewName,
                request.NewAccessLevel);

        if (!string.IsNullOrEmpty(newRoleError))
            throw new ConversionException($"Incorrect data format: {newRoleError}");

        if (user.Role.AccessLevel >= newRole.AccessLevel)
            throw new AccessException("The user does not have sufficient access rights");

        var existingRole = await _rolesService
            .GetRoleAsync(request.RoleId, cancellationToken);

        if (user.Role.AccessLevel >= existingRole.AccessLevel)
            throw new AccessException("The user does not have sufficient access rights");

        var updatedRoleId = await _rolesService
            .UpdateRoleAsync(request.RoleId, newRole, cancellationToken);

        var response = new BaseResponse<UpdateRoleResponse>(
            new UpdateRoleResponse(
                updatedRoleId),
            null);

        return Ok(response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteRole([FromQuery] DeleteRoleRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _accessCheckService.CheckAccessLevel(
            HttpContext,
            (int)AccessLevelEnumerator.Editor,
            cancellationToken);

        Console.WriteLine(request.RoleId);

        var existingRole = await _rolesService
            .GetRoleAsync(request.RoleId, cancellationToken);

        if (user.Role.AccessLevel >= existingRole.AccessLevel)
            throw new AccessException("The user does not have sufficient access rights");

        var deletedRoleId = await _rolesService
            .DeleteRoleAsync(request.RoleId, cancellationToken);

        var response = new BaseResponse<DeleteRoleResponse>(
            new DeleteRoleResponse(
                deletedRoleId),
            null);

        return Ok(response);
    }
}