using Core.Entities;
using Core.Features;

namespace Core.RepositoryContracts
{
	public interface IVisitorBlackListRepo
	{
		Task CreateVisitorBlackList(VisitorBlackList visitorBlackList);
		Task<VisitorBlackList> GetVisitorBlackListById(int id, bool trackChanges);
		Task<VisitorBlackList?> GetVisitorBlackListByNID(string Nid,bool trackChanges);
		Task<PagedList<VisitorBlackList>> GetAllVisitorsInBlackList(VisitorBlackListRequestParameters visitorBlackListRequestParameters,bool trackChanges);
		void DeleteVisitorFromBlackList(VisitorBlackList visitorBlackList);
		bool CheckIfVisitorExistsInBlackList(string  visitorNIDorPassportNum);
	}
	

}
