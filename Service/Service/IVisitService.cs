using Core.Features;
using Microsoft.AspNetCore.Http;
using Service.DTOs.Visit;
namespace Service.Service
{
    public interface IVisitService
    {
        Task<VisitForReturnDto> CreateNewVisit(VisitForCreationDto visitForCreationDto, string userId, bool trackchanges);
        Task<VisitForReturnDto> CreateNewVisitFromSecToDept(VisitForCreationFromSecertaryDto visitForCreationDto, string userIdFromToken, bool trackchanges);
        (IEnumerable<VisitForReturnDto> visitForReturnDtos, MetaData metaData ) GetAllVisits(VisitRequestParameters visitRequestParameters,bool trackchanges);
        (IEnumerable<VisitForReturnDto> visitForReturnDtos, MetaData metaData) GetVisitsToday(VisitRequestParameters visitRequestParameters);
        (IEnumerable<VisitForReturnDto> visitForReturnDtos, MetaData metaData) GetVisitsForUser(VisitRequestParameters visitRequestParameters,string UserId, bool trackchanges);
        Task<VisitForReturnDto> UpdateVisitStatusFromPolice(VisitStatusChangeFromPoliceDto visitStatusChangeDto);
        Task<VisitForReturnDto> UpdateVisitStatusFromDept(VisitStatusChangeFromDeptDto visitStatusChangeDto);
    }
	


}
