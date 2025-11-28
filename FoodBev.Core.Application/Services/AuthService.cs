using FoodBev.Application.DTOs.Authentication;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Enums;
using FoodBev.Core.Domain.Interfaces;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements the core business logic for user authentication (registration, login).
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurityService _securityService; // Dependency for hashing and tokens

        public AuthService(IUnitOfWork unitOfWork, ISecurityService securityService)
        {
            _unitOfWork = unitOfWork;
            _securityService = securityService;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegistrationDto registrationDto)
        {
            // 1. Validation: Check if email is already in use
            // IsEmailUniqueAsync returns true if email IS unique (doesn't exist)
            if (!await _unitOfWork.Users.IsEmailUniqueAsync(registrationDto.Email))
            {
                return new AuthResponseDto 
                { 
                    IsAuthenticated = false, 
                    Message = "Registration failed: Email address is already registered." 
                };
            }

            // 2. Create User Entity
            var user = new User
            {
                Email = registrationDto.Email,
                // Hash the password using the dedicated security service
                PasswordHash = _securityService.HashPassword(registrationDto.Password),
                UserType = registrationDto.UserType
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync(); // Save the User to get the generated UserID

            // 3. Create Corresponding Detail Entity (Candidate/Employer)
            if (user.UserID > 0)
            {
                if (registrationDto.UserType == UserType.Candidate)
                {
                    // For Candidate, CandidateID is the same as UserID
                    var candidate = new CandidateEntity 
                    { 
                        CandidateID = user.UserID,
                        AcceptsPOPI = false // Initialize required field
                    };
                    await _unitOfWork.Candidates.AddAsync(candidate);
                }
                else if (registrationDto.UserType == UserType.Employer)
                {
                    // For Employer, EmployerID is the same as UserID
                    var employer = new EmployerEntity 
                    { 
                        EmployerID = user.UserID,
                        UserID = user.UserID.ToString() // Set UserID as string for lookup
                    };
                    await _unitOfWork.Employers.AddAsync(employer);
                }
                
                await _unitOfWork.CompleteAsync(); // Save the detail entity

                // 4. Generate Token and Response
                var token = await _securityService.GenerateJwtTokenAsync(user);

                return new AuthResponseDto
                {
                    IsAuthenticated = true,
                    Token = token,
                    UserID = user.UserID,
                    Email = user.Email,
                    UserType = user.UserType.ToString(),
                    Message = "Registration successful."
                };
            }

            return new AuthResponseDto 
            { 
                IsAuthenticated = false, 
                Message = "Registration failed: Could not create user account." 
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 1. Retrieve User by Email
            var user = await _unitOfWork.Users.GetUserByEmailAsync(loginDto.Email);

            if (user == null || !user.IsActive)
            {
                return new AuthResponseDto 
                { 
                    IsAuthenticated = false, 
                    Message = "Login failed: Invalid credentials or inactive account." 
                };
            }

            // 2. Verify Password Hash
            if (!_securityService.VerifyPasswordHash(loginDto.Password, user.PasswordHash))
            {
                return new AuthResponseDto 
                { 
                    IsAuthenticated = false, 
                    Message = "Login failed: Invalid credentials." 
                };
            }

            // 3. Generate Token and Response
            var token = await _securityService.GenerateJwtTokenAsync(user);

            return new AuthResponseDto
            {
                IsAuthenticated = true,
                Token = token,
                UserID = user.UserID,
                Email = user.Email,
                UserType = user.UserType.ToString(),
                Message = "Login successful."
            };
        }

        public Task<AuthResponseDto> ValidateTokenAsync(string token)
        {
            // NOTE: Token validation typically happens in the infrastructure layer (middleware), 
            // but the ISecurityService should expose a method for this if needed by business logic.
            // For now, this service will defer the actual token validation implementation
            // until we build the Infrastructure (Security) project.
            
            // Placeholder: Assume validation logic from infrastructure layer returns user data
            return Task.FromResult(new AuthResponseDto 
            { 
                IsAuthenticated = false, 
                Message = "Token validation logic not yet implemented in security infrastructure." 
            });
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            if (!int.TryParse(userId, out int userID))
                return false;

            var user = await _unitOfWork.Users.GetByIdAsync(userID);
            if (user == null)
                return false;

            // Verify old password
            if (!_securityService.VerifyPasswordHash(oldPassword, user.PasswordHash))
                return false;

            // Update password
            user.PasswordHash = _securityService.HashPassword(newPassword);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
