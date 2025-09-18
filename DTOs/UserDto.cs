namespace PersonalFinanceTrackerAPI.Domain.DTOs
{
    public record UserDto
    (
        int Id,
    string FirstName,
    string LastName,
    string Email,
    string FullName,
    DateTime CreatedAt

    );

    public record CreateUserDto(
        string FirstName,
        string LastName,
        string Email,
        string PasswordHash
    );

    public record UpdateUserDto(
        string FirstName,
        string LastName,
        string Email
    );
}