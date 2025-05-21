using firstapp.Contracts.Other;

namespace firstapp.Contracts.Response.AuthorizationResponses;

public record ValidateTokenResponse(
    string Token,
    TokenDto TokenData
    );