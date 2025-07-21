using AutoMapper;
using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.RepositoryContracts;
using Service.DTOs.CardDtos;
using Service.Service;
using System.Threading.Tasks;
using static Core.Exceptions.BadRequestException;
namespace Service.Services
{
    public class CardService : ICardService
    {
        private IRepositoryManager repositoryManager;
        private IMapper mapper;
        private ILoggerManager logger;
        public CardService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger)
        {
            this.repositoryManager = repositoryManager;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<CardForReturnDto> CreateCard(CardForCreationDto cardForCreationDto, long floorId, string userId)
        {
			await CheckParentExistance(floorId, false);
			if (repositoryManager.CardRepo.ChackExistanceDeptWithTheSameNumberInSameFloor(cardForCreationDto.CardNumber,floorId))
				throw new CardAlreadyExistsBadRequestException(cardForCreationDto.CardNumber);
			var card = mapper.Map<Card>(cardForCreationDto);
            card.CreatedUserId = userId;
            await repositoryManager.CardRepo.CreateNewCard(card, floorId);

            await repositoryManager.SaveAsync();

            return mapper.Map<CardForReturnDto>(card);
        }

		public async Task DeleteCard(long cardId, string userId)
		{
			var card = await CheckExistance(cardId, true);
			card.LastModifiedUserId = userId;
			repositoryManager.CardRepo.DeleteCard(card);
			await repositoryManager.SaveAsync();
		}

		public async Task<IEnumerable<CardForReturnDto>> GetAllCardsInFloor(long floorId, bool trackchanges)
        {
            await  CheckParentExistance(floorId, trackchanges);
			var avalCards = repositoryManager.CardRepo.GetAllCardsInFloor(floorId, trackchanges);
            return mapper.Map<IEnumerable<CardForReturnDto>>(avalCards);
        }
        public async Task<CardForReturnDto> GetCardById(long Id, bool trackChanges)
        {
            var card = await CheckExistance(Id, trackChanges);
            return mapper.Map<CardForReturnDto>(card);
        }
        public async Task<IEnumerable<CardForReturnDto>> GetCardsAvaliableAsync(GetFloorCardsInDto getFloorCardsInDto /* long floorId*/, bool trackchanges)
        {
			await CheckParentExistance(getFloorCardsInDto.FloorId/*, floorId*/, trackchanges);
			var avalCards = repositoryManager.CardRepo.GetAllCardsAvalibaleInFloor(getFloorCardsInDto.FloorId,getFloorCardsInDto
                .In/*floorId*/, trackchanges);
            return mapper.Map<IEnumerable<CardForReturnDto>>(avalCards);
        }

		public async Task<CardForReturnDto> UpdateCard(CardForReturnDto cardForUpdateDto, long floorId, string userId)
		{
		 	await CheckParentExistance(floorId, false);
			var card = await CheckExistance(cardForUpdateDto.Id, true);
			if (repositoryManager.CardRepo.ChackExistanceDeptWithTheSameNumberInSameFloor(cardForUpdateDto.CardNumber, floorId ))
            {
				throw new CardAlreadyExistsBadRequestException(cardForUpdateDto.CardNumber);
			}
            mapper.Map(cardForUpdateDto, card);
            card.LastModifiedUserId = userId;
            await repositoryManager.SaveAsync();
            return mapper.Map<CardForReturnDto>(card);
		}



		#region Helper Methods
		private async Task<Card> CheckExistance(long Id, bool trackChanges)
        {
            var card = await repositoryManager.CardRepo.GetCardById(Id, trackChanges);
            if (card == null)
                throw new CardNotFoundException();
            return card;
        }

        private Task<Floor> CheckParentExistance(long floorId, bool trackChanges)
        {
            var floor = repositoryManager.FloorRepo.GetFloorById(floorId, trackChanges);
            if (floor == null)
                throw new FloorNotFoundException();
            return floor;
        } 
        #endregion
    }
}
