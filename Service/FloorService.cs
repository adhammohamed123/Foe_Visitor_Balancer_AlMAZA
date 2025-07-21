using AutoMapper;
using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.RepositoryContracts;
using Service.DTOs.FloorDtos;
using Service.Service;
using System.Threading.Tasks;
using static Core.Exceptions.BadRequestException;
namespace Service.Services
{
    public class FloorService : IFloorService
    {
        private IRepositoryManager repositoryManager;
        private IMapper mapper;
        private ILoggerManager logger;

        public FloorService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger)
        {
            this.repositoryManager = repositoryManager;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<FloorForReturnDto> CreateFloor(FloorForCreationDto forCreationDto,string userId)
        {
            if (repositoryManager.FloorRepo.CheckFloorExistsWithSameName(forCreationDto.Name))
                throw new FloorAlreadyExistsBadRequestException(forCreationDto.Name);
			
            var floor = mapper.Map<Floor>(forCreationDto);
            await repositoryManager.FloorRepo.Create(floor, userId);
            await repositoryManager.SaveAsync();
            List<Card> cards= new List<Card>();
            for(int i=forCreationDto.CardsFrom; i <= forCreationDto.CardsTo; i++)
            {
                var newCard= new Card
				{
					CardNumber = $"F{floor.Id}_{i}",
					FloorId = floor.Id,
					CreatedUserId = userId,
					CardStatus = Core.Entities.Enum.CardState.Available
				};
				cards.Add(newCard);
			}
			await repositoryManager.CardRepo.CreateCards(cards);
			await repositoryManager.SaveAsync();
			return mapper.Map<FloorForReturnDto>(floor);
        }

        public async Task DeleteAsync(long id,string userId)
        {
            var floor =await CheckExistance(id, true);
             repositoryManager.FloorRepo.DeleteFloor(floor,userId);
            var cards=repositoryManager.CardRepo.GetAllCardsInFloor(id, true);
            foreach(var card in cards)
            {
                repositoryManager.CardRepo.DeleteCard(card);
            }
            await repositoryManager.SaveAsync();
        }

        public IEnumerable<FloorForReturnDto> GetAllFloor(bool trackchanges)
        {
            var data =  repositoryManager.FloorRepo.GetAllFloorList(trackchanges);
            return mapper.Map<IEnumerable<FloorForReturnDto>>(data);
        }

		public async Task<FloorForReturnDto> GetFloorById(long Id, bool trackChanges)
		{
            var floor = await CheckExistance(Id, trackChanges);
			return mapper.Map<FloorForReturnDto>(floor);
		}

       private async Task<Floor> CheckExistance(long Id,bool trackchanges)
       {
            var f =await repositoryManager.FloorRepo.GetFloorById(Id, trackchanges);
            if (f == null)
                throw new FloorNotFoundException();
			return f;
	   }
	}
}
