using AutoMapper;
using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.RepositoryContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs.UserDtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Core.Exceptions.BadRequestException;
namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration configuration;
		private readonly RoleManager<IdentityRole> roleManager;
		private User? _user;
       // private IRepositoryManager repositoryManager;

        public UserService(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, UserManager<User> userManager, IConfiguration configuration,RoleManager<IdentityRole> roleManager)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
            this._userManager = userManager;
            this.configuration = configuration;
			this.roleManager = roleManager;
		}

      

        public async Task DeleteUser(string id, bool trackchanges)
        {
            var User = await GetObjectAndCheckExistance(id, trackchanges);
            repository.UserRepo.DeleteUser(User);
            await repository.SaveAsync();
        }

        public async Task<IEnumerable<UserForReturnDto>> GetAllUser(bool trackchanges)
        {
            List<UserForReturnDto> userData = new List<UserForReturnDto>();
			var allRoles=roleManager.Roles.ToList();
			foreach (var role in allRoles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
				foreach (var user in usersInRole)
				{
                    var userDept = await repository.DepartmentRepo.GetDepartmentByIdAsync(user.DepartmentId, trackchanges);
					var userDto = mapper.Map<UserForReturnDto>(user);
					userDto.Role = role.Name;
                    userDto.DepartmentName = userDept==null? "No Department":userDept.Name;
					userData.Add(userDto);
				}
			}
            return userData;
		}
        public async Task<UserForReturnDto?> GetFromUserById(string id, bool trackchanges)
        {
            var User = await GetObjectAndCheckExistance(id, trackchanges);
            var userDto = mapper.Map<UserForReturnDto>(User);
			return userDto;
        }
        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            var dept = await repository.DepartmentRepo.GetDepartmentByIdAsync(userForRegistration.DepartmentId, false);
            if (dept == null)
                throw new DepartmentNotFoundException(userForRegistration.DepartmentId);

            var deptContainAlreadyAccount= await repository.UserRepo.ChackExistanceUserRelatedToSameDepartment(userForRegistration.DepartmentId);
			if (deptContainAlreadyAccount)
				throw new DeptContainAlreadyAccountBadRequestException();
            
			var user = mapper.Map<User>(userForRegistration);
            user.Email = null;
            user.Id = user.DepartmentId;
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, userForRegistration.Role);
            return result;
        }
        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName);

            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
            if (!result)
                logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");
            return result;
        }

        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();

            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;

            if (populateExp)
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto(accessToken, refreshToken);
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new RefreshTokenBadRequest();
            _user = user;
            return await CreateToken(populateExp: false);
        }

     
        public async Task<IdentityResult> changePassword(ChangeUserPasswordDto changeUserPasswordDto)
        {
            var user = await _userManager.FindByIdAsync(changeUserPasswordDto.UserId);
            if (user == null)
                throw new UserNotFoundException(changeUserPasswordDto.UserId);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var operationResult = await _userManager.ResetPasswordAsync(user, token, changeUserPasswordDto.NewPassword);

            return operationResult;
        }

        #region Private Helper methods
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.NameIdentifier, _user.DepartmentId)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddDays(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
        private async Task<User> GetObjectAndCheckExistance(string id, bool trackchanges)
        {
            var User = await repository.UserRepo.GetFromUserById(id, trackchanges);
            if (User == null)
            {
                throw new UserNotFoundException(id);
            }
            return User;
        }



        #endregion



    }
}
