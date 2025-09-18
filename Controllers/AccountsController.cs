using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTrackerAPI.Application.DTOs;
using PersonalFinanceTrackerAPI.Application.Interfaces;
using PersonalFinanceTrackerAPI.Domain.Entities;

namespace PersonalFinanceTrackerAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IUserRepository userRepository, ILogger<AccountsController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(int id)
        {
            try
            {
                var account = await _userRepository.GetByIdAsync(id);
                if (account == null)
                {
                    return NotFound($"Account with ID {id} not found.");
                }

                var accountDto = new AccountDto(
                    account.Id,
                    account.Name,
                    account.AccountType,
                    account.Balance,
                    account.Currency,
                    account.CreatedAt
                );
                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }
    }
}