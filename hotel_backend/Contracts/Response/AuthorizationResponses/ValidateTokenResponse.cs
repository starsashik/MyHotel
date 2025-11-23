using hotel_backend.Contracts.Other;

namespace hotel_backend.Contracts.Response.AuthorizationResponses;

public record ValidateTokenResponse(
    string Token,
    TokenDto TokenData
    );