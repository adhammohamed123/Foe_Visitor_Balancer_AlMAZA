using Service.Services;
using Microsoft.AspNetCore.Mvc;
using Core.Entities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Core.Features;
using System.Text.Json;
using Service.DTOs.BlackListDtos;
using Presentaion.Attributes;

namespace Presentaion
{
	[Route("api/BlackList")]
	[ApiController]
	public class BlackListController : ControllerBase
	{
		private readonly IServiceManager service;
		public BlackListController(IServiceManager service)
		{
			this.service = service;
		}
		[HttpPost("BlockVisitor")]
		[Authorize(Roles = "police,nozom")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> Block([FromBody] VisitorBlockedForCreationDto visitorBlockedForCreationDto)
		{
			var  data=	await service.VisitorBlackListService.AddVisitorToBlackList(visitorBlockedForCreationDto);
			var response = new ResponseShape<VisitorBlockedDto>(StatusCodes.Status200OK, "تم حظر هذا الزائر بنجاح", null, new List<VisitorBlockedDto> { data });
			
			return Ok(response);
		}
		[HttpGet("GetAllVisitorsInBlackList")]
		[Authorize(Roles = "police,nozom")]
		public async Task<IActionResult> GetAllVisitorsInBlackList([FromQuery]VisitorBlackListRequestParameters visitorBlackListRequestParameters)
		{
			var pagedResult = await service.VisitorBlackListService.GetAllVisitorsInBlackList(visitorBlackListRequestParameters,false);
			var response = new ResponseShape<VisitorBlockedDto>(StatusCodes.Status200OK, "تم جلب الزوار المحظورين بنجاح", null, pagedResult.BlackList.ToList());
			this.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
			return Ok(response);
		}
		[HttpDelete("RemoveVisitorFromBlackList/{id}")]
		[Authorize(Roles = "police,nozom")]
		public async Task<IActionResult> RemoveVisitorFromBlackList(int id)
		{
			await service.VisitorBlackListService.RemoveVisitorFromBlackList(id);
			var response = new ResponseShape<VisitorBlockedDto>(StatusCodes.Status200OK, "تم إزالة الزائر من القائمة السوداء بنجاح", null, null);
			return Ok(response);
		}
		[HttpGet("CheckIfVisitorBlocked/{NID}")]
		[Authorize(Roles = "police,nozom,gate")]
		public IActionResult CheckIfVisitorExistsInBlackList(string NID)
		{
			var isBlocked = service.VisitorBlackListService.CheckIfVisitorExistsInBlackList(NID);
			if (isBlocked)
			{
				return Ok(new { message = "الزائر محظور" });
			}
			else
			{
				return Ok(new { message = "الزائر غير محظور" });
			}
		}
	}
}
