namespace hotel_backend.Contracts.Response.AuthorizationResponses;

public record GetCurrentUserDataResponse(
    string Token,
    string Name,
    string Email,
    int AsseccLvl,
    string ImgUrl,
    string UserId
);