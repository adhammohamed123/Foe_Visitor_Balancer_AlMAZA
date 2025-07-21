using Service.Services;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Visit;
using System.Security.Claims;
using Core.Entities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using Core.Features;
using System.Text.Json;
using Core.Entities;
using Service.DTOs.UserDtos;


namespace Presentaion
{
    [Route("api/Visit")]
    [ApiController]
    public class VisitController : ControllerBase
    {
        private readonly IServiceManager service;
        public VisitController(IServiceManager service)
        {
            this.service = service;
        }
        [HttpPost]
        [Authorize(Roles ="nozom,dept,police")]
        public async Task<IActionResult> Create([FromBody] VisitForCreationDto forCreationDto)
        {
           VisitForReturnDto data;
            // check if user contain gate role
            //if (User.IsInRole("nozom") || User.IsInRole("dept") || User.IsInRole("police"))
            //{
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                 data = await service.VisitService.CreateNewVisit(forCreationDto, userId, false);
            //}else // gate role
            //{
               
            //     data = await service.VisitService.CreateNewVisit(forCreationDto, userId, false);
            //}
                var response = new ResponseShape<VisitForReturnDto>(StatusCodes.Status200OK, "created successfuly", null, new List<VisitForReturnDto>() { data });
            return Ok(response);
        }
        [HttpPost("UnArrangedVisit")]
        [Authorize(Roles = "nozom,secertary")]
        public async Task<IActionResult> CreateNewVisitInDept([FromBody] VisitForCreationFromSecertaryDto forCreationDto)
        {
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var  data = await service.VisitService.CreateNewVisitFromSecToDept(forCreationDto, userId, false);
           
            var response = new ResponseShape<VisitForReturnDto>(StatusCodes.Status200OK, "created successfuly", null, new List<VisitForReturnDto>() { data });
            return Ok(response);
        }

        
        [Authorize(Roles = "police,nozom,FloorSecurity")]
        [HttpGet("getAllVisits")]
        public IActionResult GetAllVisits([FromQuery]VisitRequestParameters visitRequestParameters )
        {
            visitRequestParameters.VisitStateFromDept = Core.Entities.Enum.VisitState.Approved; // police get only approved dept visits and then make 
            var pagedResult = service.VisitService.GetAllVisits(visitRequestParameters,false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            var response = new ResponseShape<VisitForReturnDto>(StatusCodes.Status200OK, "ok", null, pagedResult.visitForReturnDtos.ToList());

            return Ok(response);
        }
        [Authorize(Roles = "secertary,nozom")]
        [HttpGet("GetAllVisitsCreatedBySecertary")]
        public IActionResult GetAllVisitsCreatedBySecertary([FromQuery] VisitRequestParameters visitRequestParameters)
        {
            visitRequestParameters.IsCreatedByDept = false;
            var pagedResult = service.VisitService.GetAllVisits(visitRequestParameters, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            var response = new ResponseShape<VisitForReturnDto>(StatusCodes.Status200OK, "ok", null, pagedResult.visitForReturnDtos.ToList());

            return Ok(response);
        }

        [Authorize(Roles ="gate,nozom")]
        [HttpGet("getAllVisitsToday")]
       // [SwaggerOperation("retrive all accepted visits for today this for gate man")]
        public IActionResult GetAllVisitsToday([FromQuery] VisitRequestParameters visitRequestParameters)
        {
            var data = service.VisitService.GetVisitsToday(visitRequestParameters);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(data.metaData));
            var response = new ResponseShape<VisitForReturnDto>(StatusCodes.Status200OK, "ok", null, data.visitForReturnDtos.ToList());
            return Ok(response);
        }
       
        
        [Authorize(Roles = "nozom,dept,police")]
        [HttpGet("getVisitHistoryForUser")]
       // [SwaggerOperation("retrive all visits for user(dept) account")]
        public IActionResult GetVisitHistoryForUser([FromQuery] VisitRequestParameters visitRequestParameters)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
            var pagedResult = service.VisitService.GetVisitsForUser(visitRequestParameters,userId, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            var response = new ResponseShape<VisitForReturnDto>(StatusCodes.Status200OK, "ok", null, pagedResult.visitForReturnDtos.ToList());
            return Ok(response);
        }
        
        
        [HttpPut]
        [Authorize(Roles = "nozom,police")]
        public async Task<IActionResult> UpdateVisitStatus(VisitStatusChangeFromPoliceDto change)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var data=await service.VisitService.UpdateVisitStatusFromPolice(change,userId);
            var response=new ResponseShape<VisitForReturnDto>(StatusCodes.Status200OK, "updated successfuly", null, new List<VisitForReturnDto>() { data });
            return Ok(response);
        }
       
        
        [HttpPut("deptAcceptance")]
        [Authorize(Roles = "nozom,dept")]
        public async Task<IActionResult> UpdateVisitStatusFromdept(VisitStatusChangeFromDeptDto change)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var data = await service.VisitService.UpdateVisitStatusFromDept(change,userId);
            var response = new ResponseShape<VisitForReturnDto>(StatusCodes.Status200OK, "updated successfuly", null, new List<VisitForReturnDto>() { data });
            return Ok(response);
        }
    }
}
