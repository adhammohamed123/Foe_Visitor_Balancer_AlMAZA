using Service.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.DepartmentDtos
{
    public class DepartmentForCreationDto
    {
        public string Name { get; set; }
    }

    public class DepartmentForReturnDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
