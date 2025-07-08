using AutoMapper;
using Core.Contracts;
using Core.Entities;
using Core.Entities.Enum;
using Core.Exceptions;
using Core.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.Visitor;
using Service.Service;
using static Core.Exceptions.BadRequestException;
namespace Service.Services
{
    public class VisitorService : IVisitorService
    {
        private IRepositoryManager repositoryManager;
        private IMapper mapper;
        private ILoggerManager logger;

        public VisitorService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger)
        {
            this.repositoryManager = repositoryManager;
            this.mapper = mapper;
            this.logger = logger;
        }

		public async Task<VisitorForReturnDto> AssignCardToVisitor(VisitorTakeCardInSpecifecVisitDto visitorTakeCardInSpecifecVisitDto)
		{
            await CheckVisitExistance(visitorTakeCardInSpecifecVisitDto.VisitId, false);
			var visitor = await CheckVisitorExistance(visitorTakeCardInSpecifecVisitDto.Id, true);
            if(visitor.IsBloacked)
                throw new VisitorBlockedBadRequsetException(visitor.NID);
			var card = await CheckCardExistanceAndAvailable(visitorTakeCardInSpecifecVisitDto.CardId, true);
			
            if (visitor.CardId != card.Id && visitor.CardId != null)
            {
                var oldCard = await CheckCardExistance(visitor.CardId, true);
                oldCard.CardStatus = CardState.Available;
			}
		    
            repositoryManager.VisitorRepo.AssignCardToVisitor(visitor, card.Id);
            repositoryManager.CardRepo.CheckInCardForVisit(card);
            await repositoryManager.SaveAsync();
			return mapper.Map<VisitorForReturnDto>(visitor);
		}

		public async Task<VisitorForReturnDto> CreateNewVisitorInVisit(VisitorForCreationDto forCreationDto, long VisitId, string userId,bool trackchanges)
        {
            var visit=await CheckVisitExistance(VisitId, trackchanges);
            if(visit.VisitStateFromPolice == VisitState.Approved)
			{
				throw new CannotAddVisitorToAcceptedVisitBadRequestException("مسئولي الامن");
			}else if(visit.VisitStateFromPolice == VisitState.Rejected)
			{
				throw new CannotAddVisitorToRejectedVisitBadRequestException("مسئولي الامن");
			}
            if(visit.CreatedUserId != userId) // secertary is now try to add new visitor 
            {
                if (visit.VisitStateFromDept == VisitState.Approved)
                {
                    throw new CannotAddVisitorToAcceptedVisitBadRequestException("مسئولي الاداره");
                }
                else if (visit.VisitStateFromDept == VisitState.Rejected)
                {
                    throw new CannotAddVisitorToRejectedVisitBadRequestException("مسئولي الاداره");
                }
            }
			var visitor= mapper.Map<Visitor>(forCreationDto);
            visitor.CreatedUserId = userId;
            if (repositoryManager.BlackListRepo.CheckIfVisitorExistsInBlackList(visitor.NID))
                repositoryManager.VisitorRepo.BlockVisitor(visitor);
			await repositoryManager.VisitorRepo.CreateVisitor(VisitId, visitor);
            var dept= await repositoryManager.DepartmentRepo.GetDepartmentByIdAsync(userId, false);
	        visitor.NID_PicPath=await repositoryManager.StorageService.SaveVisitorNidAsync(forCreationDto.NID_PicPath,dept.Name, visit.CreatedDate);
            await repositoryManager.SaveAsync();
            return mapper.Map<VisitorForReturnDto>(visitor);
        }

        public async Task<IEnumerable<VisitorForReturnDto>> GetAllVisitors(bool trackchanges)
        {
            var data= await repositoryManager.VisitorRepo.GetAllVisitors(trackchanges);
            return mapper.Map<IEnumerable<VisitorForReturnDto>>(data);
        }

        public async Task<IEnumerable<VisitorForReturnDto>> GetAllVisitorsInSpecificVisit(long visitId)
        {
           var data=await repositoryManager.VisitorRepo.GetVisitorsInVisit(visitId,false).ToListAsync();
            return mapper.Map<IEnumerable<VisitorForReturnDto>>(data) ;
        }

        private async Task<Visit> CheckVisitExistance(long visitId, bool trackChanges)
		{
			var visit = await repositoryManager.VisitRepo.GetVisitById(visitId, trackChanges);
			if (visit == null)
			{
				throw new VisitNotFoundException(visitId);
			}
			return visit;
		}
        private async Task<Visitor> CheckVisitorExistance(long visitorId, bool trackChanges)
        {
            var visitor = await repositoryManager.VisitorRepo.GetVisitorById(visitorId, trackChanges);
            if (visitor == null)
            {
                throw new VisitorNotFoundException();
            }
            return visitor;
        }
        private async Task<Card> CheckCardExistanceAndAvailable(long? cardId, bool trackChanges)
        {
            var card = await CheckCardExistance(cardId, trackChanges);
			if (card.CardStatus == Core.Entities.Enum.CardState.NotAvailable)
			{
				throw new CardAlreadyAssignedException();
			}
			return card;
		}
        private async Task<Card> CheckCardExistance(long? cardId, bool trackChanges)
        {
            if (cardId == null)
                throw new CardNotFoundException();
            var card = await repositoryManager.CardRepo.GetCardById(cardId??0, trackChanges);
            if (card == null)
                throw new CardNotFoundException();
            return card;
        }

		public async Task<VisitorForReturnDto> RegisterEntryTime(long Id, bool trackchanges)
		{
			var visitor=await CheckVisitorExistance(Id, trackchanges);
            if(visitor.CardId == null)
			{
				throw new VisitorNotHaveCardToEnterBadRequest();
			}
            if (visitor.IsBloacked)
            {
                var card= await CheckCardExistance(visitor.CardId??0, true);
                card.CardStatus = CardState.Available;
                await repositoryManager.SaveAsync();
                throw new VisitorBlockedBadRequsetException(visitor.NID);
            }
            repositoryManager.VisitorRepo.RegisterEntryTime(visitor);
			await repositoryManager.SaveAsync();
			return mapper.Map<VisitorForReturnDto>(visitor);
		}
		public async Task<VisitorForReturnDto> RegisterLeaveTime(long Id, bool trackchanges)
		{
			var visitor = await CheckVisitorExistance(Id, trackchanges);
            if (visitor.IsBloacked)
            {
                var Vcard = await CheckCardExistance(visitor.CardId ?? 0, true);
                Vcard.CardStatus = CardState.Available;
                await repositoryManager.SaveAsync();
                throw new VisitorBlockedBadRequsetException(visitor.NID);
            }
            repositoryManager.VisitorRepo.RegisterLeaveTime(visitor);
            var card = await CheckCardExistance(visitor.CardId, trackchanges);
            card.CardStatus = CardState.Available;
			await repositoryManager.SaveAsync();
			return mapper.Map<VisitorForReturnDto>(visitor);
		}

        
	}
}
