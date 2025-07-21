using Service.Services;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.FloorDtos;
using System.Security.Claims;
using Presentaion.Attributes;
using Core.Entities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Presentaion
{
    [Route("api/Floor")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly IServiceManager service;
        public FloorController(IServiceManager service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = "dept,nozom,police,gate,secertary,FloorSecurity")]
        //[SwaggerOperation("Get all floors")]
		public async Task<IActionResult> Get()
        {
           var data=  service.FloorService.GetAllFloor(false);
            var response = new ResponseShape<FloorForReturnDto>(StatusCodes.Status200OK, "ok", null,  data.ToList() );
            return Ok(response);
        }
        [HttpGet]
        [Route("{Id:long}")]
        public async Task<IActionResult> Get(long Id)
        {
			var data = await service.FloorService.GetFloorById(Id, false);
			var response = new ResponseShape<FloorForReturnDto>(StatusCodes.Status200OK, "ok", null, new List<FloorForReturnDto>() { data });
			return Ok(response);
		}



		[HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles ="nozom")]
        //[SwaggerOperation("Create a new floor")]
		public async Task<IActionResult> Create([FromBody] FloorForCreationDto dto) { 
        
           var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           var data = await service.FloorService.CreateFloor(dto,userId);
            var response= new ResponseShape<FloorForReturnDto>(StatusCodes.Status200OK, "Created Successfuly", null,new List<FloorForReturnDto>(){ data }); 
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "nozom")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await service.FloorService.DeleteAsync(id,userId);
            return Ok("Deleted Successfully");
        }
    }
}
