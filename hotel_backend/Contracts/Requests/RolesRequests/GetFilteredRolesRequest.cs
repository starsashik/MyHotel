namespace hotel_backend.Contracts.Requests.RolesRequests;

public record GetFilteredRolesRequest(
    string? PartOfName,
    int? AccessLevelFrom,
    int? AccessLevelTo
);