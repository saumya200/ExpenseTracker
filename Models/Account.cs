using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Transactions;

namespace PersonalFinanceTrackerAPI.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public AccountType AccountType { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }

        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }

    public enum AccountType
    {
        Checking = 1,
        Savings = 2,
        CreditCard = 3,
        Investment = 4,
        Cash = 5
    }
}