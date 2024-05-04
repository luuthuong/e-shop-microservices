namespace Domain;

public sealed record CustomerData(
    string Email,
    string Name,
    Address? Address,
    CreditLimit? CreditLimit
);