using Core.Entities.Enum;

namespace Service.DTOs.CardDtos
{
	public class CardForCreationDto
    {
        public CardState CardStatus { get; set; }
		public string? CardNumber { get; set; }

	}
    //public record GetFloorCardsInDto
    //{
    //    public long FloorId { get; set; }
    //    public DateTime In { get; set; }
    //}
}
