using Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Presentaion.Attributes;
using Core.Entities.Response;
using Microsoft.AspNetCore.Http;
using Service.DTOs.CardDtos;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentaion
{
    [Route("api/Cards")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IServiceManager service;
        public CardsController(IServiceManager service)
        {
            this.service = service;
        }

        [HttpGet("getAllAvailableCardsInFloor/{floorId:long}")]
        [Authorize(Roles ="police")]
        //[SwaggerOperation("Get all cards for a specific floor")]
		public async Task<IActionResult> Get(long floorId)
        {
            var data = await service.CardService.GetCardsAvaliableAsync(floorId,false);
            var response = new ResponseShape<CardForReturnDto>(StatusCodes.Status200OK, "ok", null, data.ToList());
            return Ok(response);
        }
		[HttpGet("getAllCardsInFloor/{floorId:long}")]
		[Authorize(Roles = "nozom")]
		//[SwaggerOperation("Get all cards for a specific floor")]
		public async Task<IActionResult> GetAll(long floorId)
		{
			var data = await service.CardService.GetAllCardsInFloor(floorId, false);
			var response = new ResponseShape<CardForReturnDto>(StatusCodes.Status200OK, "ok", null, data.ToList());
			return Ok(response);
		}
		[HttpGet("getCard/{Id:long}")]
		[Authorize(Roles = "police,nozom,gate")]
		public async Task<IActionResult> GetCard(long Id)
		{
			var data = await service.CardService.GetCardById(Id, false);
			var response = new ResponseShape<CardForReturnDto>(StatusCodes.Status200OK, "ok", null, new List<CardForReturnDto>() { data });
			return Ok(response);
		}

		[HttpPost("CreateInFloor/{floorId:long}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles ="police,nozom")]
        public async Task<IActionResult> Create(CardForCreationDto dto,long floorId) {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var data=   await  service.CardService.CreateCard(dto,floorId,userId);
            var response= new ResponseShape<CardForReturnDto>(StatusCodes.Status200OK,"created successfully",null, new List<CardForReturnDto>() { data});
            return Ok(response);
        }
		[HttpPut("UpdateCard/{floorId:long}")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "police,nozom")]
		public async Task<IActionResult> UpdateCard(CardForReturnDto dto,long floorId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var updatedCard= await service.CardService.UpdateCard(dto, floorId,userId);
			var response = new ResponseShape<CardForReturnDto>(StatusCodes.Status200OK, "updated successfully", null, new List<CardForReturnDto>() { updatedCard });
			return Ok(response);
		}
		[HttpDelete("{cardId:long}")]
		[Authorize(Roles = "nozom")]
		public async Task<IActionResult> Delete(long cardId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			await service.CardService.DeleteCard(cardId, userId);
			var response = new ResponseShape<object>(StatusCodes.Status200OK, "deleted successfully", null, null);
			return Ok(response);
		}


	}
}
