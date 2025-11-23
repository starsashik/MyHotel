namespace hotel_backend.Contracts.Requests.HotelsRequests;

public record GetFilteredHotelsRequest(
    string? PartOfName,
    string? PartOfLocation,
    string? PartOfDescription
    );