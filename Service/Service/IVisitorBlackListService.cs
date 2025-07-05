using Core.Features;
using Service.DTOs.BlackListDtos;

namespace Service.Service
{
	public interface IVisitorBlackListService
	{
		bool CheckIfVisitorExistsInBlackList(string NID);
		Task<VisitorBlockedDto> AddVisitorToBlackList(string NID);
		Task RemoveVisitorFromBlackList(int Id);
		Task<(IEnumerable<VisitorBlockedDto> BlackList, MetaData metaData)> GetAllVisitorsInBlackList(VisitorBlackListRequestParameters visitorBlackListRequestParameters, bool trackchanges);


    }
}
