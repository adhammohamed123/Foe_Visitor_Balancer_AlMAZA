using Service.DTOs.Visitor;

namespace Service.Service
{
	public interface IVisitorService
	{
		Task<VisitorForReturnDto> CreateNewVisitorInVisit(VisitorForCreationDto forCreationDto, long VisitId, string userId, bool trackchanges);
		Task<VisitorForReturnDto> AssignCardToVisitor(VisitorTakeCardInSpecifecVisitDto visitorTakeCardInSpecifecVisitDto);
		Task<IEnumerable<VisitorForReturnDto>> GetAllVisitors(bool trackchanges);
		Task<IEnumerable<VisitorForReturnDto>> GetAllVisitorsInSpecificVisit(long visitId);
		Task<VisitorForReturnDto> RegisterEntryTime(long Id, bool trackchanges);
		Task<VisitorForReturnDto> RegisterLeaveTime(long Id, bool trackchanges);

	}
}
