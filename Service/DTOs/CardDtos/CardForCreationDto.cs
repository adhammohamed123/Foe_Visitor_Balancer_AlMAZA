using Core.Entities.Enum;

namespace Service.DTOs.CardDtos
{
	public class CardForCreationDto
    {
        public CardState CardStatus { get; set; }
		public string? CardNumber { get; set; }

	}
}
