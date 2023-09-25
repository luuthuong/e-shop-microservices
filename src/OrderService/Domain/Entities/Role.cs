using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class Role: IdentityRole<Guid>
{
    private Role() { }

    private Role(string name, string normalizedName = "") => (Name,NormalizedName) = (name, normalizedName);

    public static Role Create(string name, string normalizedName = "")
    {
        bool isValid = ValidCreate(name, normalizedName);
        if (isValid)
            throw new ValidationException($"{nameof(Name)} role invalid.");
        return new Role(name, normalizedName);
    }

    private static bool ValidCreate(string name, string normalizedName = "")
    {
        return true;
    }
}