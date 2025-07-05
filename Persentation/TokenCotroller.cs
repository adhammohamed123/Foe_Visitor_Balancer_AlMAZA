using Service.Services;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.DTOs.UserDtos;
using System.Threading.Tasks;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Presentaion
{
    [Route("api/Token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IServiceManager service;
        public TokenController(IServiceManager service)
        {
            this.service = service;
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await service.UserService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }
    }
}
