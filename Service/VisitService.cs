using AutoMapper;
using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.Features;
using Core.RepositoryContracts;
using Service.DTOs.Visit;
using Service.Service;
using static Core.Exceptions.BadRequestException;
namespace Service.Services
{
    public class VisitService : IVisitService
    {
        private IRepositoryManager repositoryManager;
        private IMapper mapper;
        private ILoggerManager logger;

        public VisitService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger)
        {
            this.repositoryManager = repositoryManager;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<VisitForReturnDto> CreateNewVisit(VisitForCreationDto visitForCreationDto, string userIdFromToken,bool trackchanges)
        {
            
            var visit=mapper.Map<Visit>(visitForCreationDto);
            await repositoryManager.VisitRepo.CreateNewVisit(visit,userIdFromToken);
            visit.IsCreatedByDept = true;
            visit.VisitStateFromDept = Core.Entities.Enum.VisitState.Approved;
            visit.VisitDate = visitForCreationDto.PrimaryDate;
            await repositoryManager.SaveAsync();
            var result = await repositoryManager.VisitRepo.GetVisitDetailsWithVisitors(visit.Id, trackchanges);
            var data = mapper.Map<VisitForReturnDto>(result);
			return data ;
        }

        public async Task<VisitForReturnDto> CreateNewVisitFromSecToDept(VisitForCreationFromSecertaryDto visitForCreationDto, string userIdFromToken, bool trackchanges)
        {

            var visit = mapper.Map<Visit>(visitForCreationDto);
            visit.VisitDate=visitForCreationDto.PrimaryDate;
            await repositoryManager.VisitRepo.Create(visit);
            if(visit.CreatedUserId != userIdFromToken)
            {
                visit.IsCreatedByDept = false;
            }
            await repositoryManager.SaveAsync();
            var result = await repositoryManager.VisitRepo.GetVisitDetailsWithVisitors(visit.Id, trackchanges);
            var data = mapper.Map<VisitForReturnDto>(result);
            return data;
        }

        public (IEnumerable<VisitForReturnDto> visitForReturnDtos, MetaData metaData) GetAllVisits(VisitRequestParameters visitRequestParameters,bool trackchanges)
        {
            var data= repositoryManager.VisitRepo.GetAllVisits(visitRequestParameters,trackchanges);
            var visits= mapper.Map<IEnumerable<VisitForReturnDto>>(data);
            return(visits, data.metaData);
        }
     

        public (IEnumerable<VisitForReturnDto> visitForReturnDtos, MetaData metaData) GetVisitsForUser(VisitRequestParameters visitRequestParameters,string UserId, bool trackchanges)
        {
            var data=repositoryManager.VisitRepo.GetVisitsForUser(visitRequestParameters,UserId, trackchanges);
            var visits = mapper.Map<IEnumerable<VisitForReturnDto>>(data);
            return (visits,data.metaData);
        }

        public (IEnumerable<VisitForReturnDto> visitForReturnDtos, MetaData metaData) GetVisitsToday(VisitRequestParameters visitRequestParameters)
        {
            var data=repositoryManager.VisitRepo.GetAllVisitsInThisDay(visitRequestParameters);
            var visits = mapper.Map<IEnumerable<VisitForReturnDto>>(data);
            return (visitForReturnDtos:visits,data.metaData);
        }

        public async Task<VisitForReturnDto> UpdateVisitStatusFromPolice(VisitStatusChangeFromPoliceDto visitStatusChangeDto,string userId)
        {
            var visit=await repositoryManager.VisitRepo.GetVisitById(visitStatusChangeDto.Id,true);
            if (visit == null)
                throw new VisitNotFoundException(visitStatusChangeDto.Id);

             //   if(visitStatusChangeDto.VisitState == Core.Entities.Enum.VisitState.Approved)
             //   {
             //      if (await repositoryManager.VisitorRepo.GetVisitorsInVisit(visit.Id, false).AnyAsync(visitor=> visitor.CardId == null))
			 //		    throw new CannotApproveVisitWithoutCardException();
			 //   }

            mapper.Map(visitStatusChangeDto, visit);
            if(visitStatusChangeDto.VisitStateFromPolice == Core.Entities.Enum.VisitState.Approved)
            {
                visit.ReasonforRejection=null;
                if (visitStatusChangeDto.IsPraimaryDateAccepted == true)
                {
                    visit.VisitDate = visit.PrimaryDate;
                }
                else
                {
                    visit.VisitDate = visit.SecondaryDate ?? visit.PrimaryDate;
                }

            }else  {
                visit.IsPraimaryDateAccepted = null;
            }
            visit.LastModifiedUserId = userId;
            await repositoryManager.SaveAsync();
            return mapper.Map<VisitForReturnDto>(visit);
        }

        public async Task<VisitForReturnDto> UpdateVisitStatusFromDept(VisitStatusChangeFromDeptDto visitStatusChangeDto,string userId)
        {

            var visit = await repositoryManager.VisitRepo.GetVisitById(visitStatusChangeDto.Id, true);
            if (visit == null)
                throw new VisitNotFoundException(visitStatusChangeDto.Id);
            if(visit.VisitStateFromPolice != Core.Entities.Enum.VisitState.Pending)
                throw new CannotUpdateVisitStateAfterPoliceTakeActionBadRequestException();
            mapper.Map(visitStatusChangeDto, visit);
            await repositoryManager.SaveAsync();
            return mapper.Map<VisitForReturnDto>(visit);
        }

        private async Task<Department> CheckDepartmentExistance(string departmentId, bool trackChanges)
		{
			var department = await repositoryManager.DepartmentRepo.GetDepartmentByIdAsync(departmentId, trackChanges);
			if (department == null)
				throw new DepartmentNotFoundException(department.Id);
			return department;
		}
	}
}
