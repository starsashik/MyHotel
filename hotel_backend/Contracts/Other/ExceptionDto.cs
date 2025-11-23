namespace hotel_backend.Contracts.Other;

public record ExceptionDto(
    string ErrorGroup,
    string ErrorMessage
);