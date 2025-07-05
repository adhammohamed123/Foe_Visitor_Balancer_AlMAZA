using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.UserDtos
{
    public record TokenDto(string AccessToken, string RefreshToken);
}
