using Core.Results;

namespace Domain;

public static class CustomerErrors
{
    public static Error NameIsNullOrEmpty => new("Customer.NameIsNullOrEmpty", "Customer can't be null or empty");
}
