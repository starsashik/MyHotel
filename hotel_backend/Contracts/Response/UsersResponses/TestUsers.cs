namespace hotel_backend.Contracts.Response.UsersResponses;

public record TestUsers(
    string UserId,
    string Email,
    string Name,
    string Role,
    string ImgUrl
);