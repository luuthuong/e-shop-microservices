using Core.Mediator;

namespace Application.DTOs;

public record PaymentMethodDTO(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedDate,
    DateTime? UpdatedDate
);

public record AddPaymentMethodRequest(
    string Name,
    string Description = "");

public record AddPaymentMethodResponse: BaseResponse<PaymentMethodDTO>;