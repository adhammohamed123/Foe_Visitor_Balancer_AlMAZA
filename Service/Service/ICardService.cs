using Service.DTOs.CardDtos;

namespace Service.Service
{
    public interface ICardService
    {
        Task<IEnumerable<CardForReturnDto>> GetCardsAvaliableAsync(long floorId,bool trackchanges);
        Task<IEnumerable<CardForReturnDto>> GetAllCardsInFloor(long floorId, bool trackchanges);
		Task<CardForReturnDto> GetCardById(long Id, bool trackChanges);
		Task<CardForReturnDto> CreateCard(CardForCreationDto cardForCreationDto,long floorId ,string userId);
        Task<CardForReturnDto> UpdateCard(CardForReturnDto cardForUpdateDto, long floorId, string userId);
        Task DeleteCard(long cardId, string userId);

	}
}
