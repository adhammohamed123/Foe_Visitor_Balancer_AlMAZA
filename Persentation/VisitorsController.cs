using Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Entities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Service.DTOs.Visitor;
using System.Threading.Tasks;


namespace Presentaion
{
    [Route("api/Visitors")]
    [ApiController]
    public class VisitorsController : ControllerBase
    {
        private readonly IServiceManager service;
        public VisitorsController(IServiceManager service)
        {
            this.service = service;
        }
        [HttpPost("{visitId:long}")]
        //[SwaggerOperation("Create a new visitor in a specific visit for loggedIn User(dept) account")]
        [Authorize(Roles = "nozom,dept,secertary")]
        public async Task<IActionResult> Create(long visitId, [FromForm] VisitorForCreationDto forCreationDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var data = await service.VisitorService.CreateNewVisitorInVisit(forCreationDto, visitId, userId, false);

            var response = new ResponseShape<VisitorForReturnDto>(StatusCodes.Status200OK, "created successfuly", null, new List<VisitorForReturnDto>() { data });
            return Ok(response);
        }

        [HttpGet("getAllVisitors")]
        //[SwaggerOperation("Retrieve all visitors for police department")]
        [Authorize(Roles = "nozom,dept,police")]
        public async Task<IActionResult> GetAllVisitors()
        {
            var data = await service.VisitorService.GetAllVisitors(false);
            var response = new ResponseShape<VisitorForReturnDto>(StatusCodes.Status200OK, "ok", null, data.ToList());
            return Ok(response);
        }
        [HttpGet("In/{visitId:long}")]
        [Authorize(Roles = "nozom,dept,police")]
        public async Task<IActionResult> GetAllVisitorsinVisit(long visitId)
        {
            var data = await service.VisitorService.GetAllVisitorsInSpecificVisit(visitId);
            var response = new ResponseShape<VisitorForReturnDto>(StatusCodes.Status200OK, "ok", null, data.ToList());
            return Ok(response);
        }
        [HttpPut("AssignCardToVisitor")]
        [Authorize(Roles = "nozom,police")]
        public async Task<IActionResult> AssignCardToVisitor(VisitorTakeCardInSpecifecVisitDto visitorTakeCardInSpecifecVisitDto)
        {
            var data = await service.VisitorService.AssignCardToVisitor(visitorTakeCardInSpecifecVisitDto);
            var response = new ResponseShape<VisitorForReturnDto>(StatusCodes.Status200OK, "Card assigned successfully", null, new List<VisitorForReturnDto>() { data });
            return Ok(response);
        }
        [HttpPut("RegisterEntryTime/{Id:long}")]
        [Authorize(Roles = "nozom,gate")]
		public async Task<IActionResult> RegisterEntryTime(long Id)
        {
            var data = await service.VisitorService.RegisterEntryTime(Id, true);
            var response = new ResponseShape<VisitorForReturnDto>(StatusCodes.Status200OK, "تم تسجيل دخول بنجاح", null, new List<VisitorForReturnDto>() { data });
            return Ok(response);
        }
        [HttpPut("RegisterLeaveTime/{Id:long}")]
		[Authorize(Roles = "nozom,gate")]
		public async Task<IActionResult> RegisterLeaveTime(long Id)
        {
            var data = await service.VisitorService.RegisterLeaveTime(Id, true);
            var response = new ResponseShape<VisitorForReturnDto>(StatusCodes.Status200OK, "تم تسجيل خروج بنجاح", null, new List<VisitorForReturnDto>() { data });
            return Ok(response);
        }
    }
}

