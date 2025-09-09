using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTrackerAPI.Application.Interfaces;
using PersonalFinanceTrackerAPI.Domain.DTOs;
using PersonalFinanceTrackerAPI.Domain.Entities;

namespace PersonalFinanceTrackerAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                var userDto = new UserDto(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.FullName,
                    user.CreatedAt
                );
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                if (await _userRepository.EmailExistsAsync(createUserDto.Email))
                {
                    return BadRequest("Email already in use.");
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.PasswordHash);

                var user = new User
                {
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    Email = createUserDto.Email,
                    PasswordHash = passwordHash
                };

                var createdUser = await _userRepository.CreateAsync(user);
                var userDto = new UserDto(
                    createdUser.Id,
                    createdUser.FirstName,
                    createdUser.LastName,
                    createdUser.Email,
                    createdUser.FullName,
                    createdUser.CreatedAt
                );
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, userDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                if (user.Email != updateUserDto.Email && await _userRepository.EmailExistsAsync(updateUserDto.Email))
                {
                    return BadRequest("Email address is already in use.");
                }

                user.FirstName = updateUserDto.FirstName;
                user.LastName = updateUserDto.LastName;
                user.Email = updateUserDto.Email;

                var updateUserDto = await _userRepository.UpdateAsync(user);
                var userDto = new UserDto(
                    updateUserDto.Id,
                    updateUserDto.FirstName,
                    updateUserDto.LastName,
                    updateUserDto.Email,
                    updateUserDto.FullName,
                    updateUserDto.CreatedAt
                );
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", id);
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (!await _userRepository.ExistsAsync(id))
                {
                    return NotFound($"User with ID {id} not found.");
                }

                await _userRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }
    }
}