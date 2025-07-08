using AutoMapper;
using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.Features;
using Core.RepositoryContracts;
using Service.DTOs.BlackListDtos;
using Service.Service;
using static Core.Exceptions.BadRequestException;

namespace Service
{
	public class VisitorBlackListService : IVisitorBlackListService
	{
		private IRepositoryManager repositoryManager;
		private IMapper mapper;
		private ILoggerManager logger;

		public VisitorBlackListService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger)
		{
			this.repositoryManager = repositoryManager;
			this.mapper = mapper;
			this.logger = logger;
		}

		public async Task<VisitorBlockedDto> AddVisitorToBlackList(VisitorBlockedForCreationDto visitorBlockedForCreationDto)
		{
			if (repositoryManager.BlackListRepo.CheckIfVisitorExistsInBlackList(visitorBlockedForCreationDto.VisitorIdentifierNIDorPassportNumber))
			{
				throw new VisitorAlreadyBlockedBadRequestException(visitorBlockedForCreationDto.VisitorIdentifierNIDorPassportNumber);
				//var blocked =await repositoryManager.BlackListRepo.GetVisitorBlackListByNID(NID,false);
				//return mapper.Map<VisitorBlockedDto>(blocked);
			}
			var BlockedVisitor = mapper.Map<VisitorBlackList>(visitorBlockedForCreationDto);
			await repositoryManager.BlackListRepo.CreateVisitorBlackList(BlockedVisitor);
			var visitorInstances = await repositoryManager.VisitorRepo.GetVisitorInstancesByNID(visitorBlockedForCreationDto.VisitorIdentifierNIDorPassportNumber, true);
			//repositoryManager.VisitorRepo.BlockAndUnBlockVisitorInAllInstancesusingSql(visitorBlockedForCreationDto.VisitorIdentifierNIDorPassportNumber,isBlocked:true);

			var lastVisitorRecord= visitorInstances.Where(v=>v.EntryTime==null);

			//var lastVisitorRecord=await repositoryManager.VisitorRepo.GetLatestVisitorRecordByNid(visitorBlockedForCreationDto.VisitorIdentifierNIDorPassportNumber,true);
			
			
			foreach (var lR in lastVisitorRecord) {
			
				if (lR.CardId!=null)
				{
					var card = await repositoryManager.CardRepo.GetCardById(lR.CardId??0, true);
					card.CardStatus = Core.Entities.Enum.CardState.Available;
					lR.CardId=null;
				}
			}
			foreach (var visitor in visitorInstances)
			{
				repositoryManager.VisitorRepo.BlockVisitor(visitor);
			}
			await repositoryManager.SaveAsync();
			return mapper.Map<VisitorBlockedDto>(BlockedVisitor);
		}

		public bool CheckIfVisitorExistsInBlackList(string NID)
		=> repositoryManager.BlackListRepo.CheckIfVisitorExistsInBlackList(NID);



		public async Task<(IEnumerable<VisitorBlockedDto> BlackList,MetaData metaData)> GetAllVisitorsInBlackList(VisitorBlackListRequestParameters visitorBlackListRequestParameters,bool trackchanges)
		{
			var Entities=await repositoryManager.BlackListRepo.GetAllVisitorsInBlackList(visitorBlackListRequestParameters,trackchanges);
			var data= mapper.Map<IEnumerable<VisitorBlockedDto>>(Entities);
			return (BlackList: data, metaData: Entities.metaData);
		}

		public async Task RemoveVisitorFromBlackList(int Id)
		{
			// get from blacklist
			var blockedEntity = await repositoryManager.BlackListRepo.GetVisitorBlackListById(Id, true);
			if(blockedEntity == null)
			{
				throw new VisitorBlackListNotFoundException();
		    }
			var visitorInstances = await repositoryManager.VisitorRepo.GetVisitorInstancesByNID(blockedEntity.VisitorIdentifierNIDorPassportNumber, true);
			foreach (var visitor in visitorInstances)
			{
				repositoryManager.VisitorRepo.UnBlockVisitor(visitor);
			}

			//repositoryManager.VisitorRepo.BlockAndUnBlockVisitorInAllInstancesusingSql(blockedEntity.VisitorIdentifierNIDorPassportNumber, isBlocked: false);
		    repositoryManager.BlackListRepo.DeleteVisitorFromBlackList(blockedEntity);
			await repositoryManager.SaveAsync();
		}
	}
}