using PersonalFinanceTrackerAPI.Domain.Entities;

namespace PersonalFinanceTrackerAPI.Application.DTOs
{
    public record AccountDto(
        int Id,
        string Name,
        AccountType AccountType,
        decimal Balance,
        string Currency,
        DateTime CreatedAt
    );

    public record CreateAccountDto(
        string Name,
        AccountType AccountType,
        decimal InitialBalance,
        string Currency = "USD"
    );

    public record UpdateAccountDto(
        string Name,
        AccountType AccountType
    );
}