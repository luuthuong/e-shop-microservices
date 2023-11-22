using Core.Domain;

namespace Domain.Entities;



public class PaymentOption: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PaymentOptionType Type { get; set; }
}

public enum PaymentOptionType
{
    None,
    PayCard,
    PayGiftCard,
    PayLoyaltyCard
}