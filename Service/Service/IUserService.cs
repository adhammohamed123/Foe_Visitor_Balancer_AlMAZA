using Microsoft.AspNetCore.Identity;
using Service.DTOs.UserDtos;
using System.Threading.Tasks;
namespace Service.Services
{
    public interface IUserService
    {
		Task<IEnumerable<UserForReturnDto>> GetAllUser(bool trackchanges);
        //Task<IEnumerable<UserDto>>GetAllUsersNamesAndIds(bool trackchanges);
        Task<UserForReturnDto?> GetFromUserById(string id, bool trackchanges);
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateToken(bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
        Task DeleteUser(string id,bool trackchanges);
        Task<IdentityResult> changePassword(ChangeUserPasswordDto changeUserPasswordDto);

    }
}
