using firstapp.Contracts.Other;

namespace firstapp.Contracts;

public record BaseResponse<T>(
    T? Data,
    ExceptionDto? ErrorMessage
);