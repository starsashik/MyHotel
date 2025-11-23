using hotel_backend.Contracts.Other;

namespace hotel_backend.Contracts;

public record BaseResponse<T>(
    T? Data,
    ExceptionDto? ErrorMessage
);