namespace firstapp.Contracts.Requests.RolesRequests;

public record GetFilteredRolesRequest(
    string? PartOfName,
    int? AccessLevelFrom,
    int? AccessLevelTo
);