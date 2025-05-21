namespace firstapp.Contracts.Requests.HotelsRequests;

public record GetFilteredHotelsRequest(
    string? PartOfName,
    string? PartOfLocation,
    string? PartOfDescription
    );