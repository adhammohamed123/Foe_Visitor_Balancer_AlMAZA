using Core.Entities;
using Core.Features;
using Core.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repository.Extensions;

namespace Repository
{
	public  class VisitorBlackListRepo:BaseRepository<VisitorBlackList>, IVisitorBlackListRepo
	{
		

		public VisitorBlackListRepo(FoeVisitContext context):base(context)
		{
			
		}

		public bool CheckIfVisitorExistsInBlackList(string visitorNIDorPassportNum)
		=>FindByCondition(v => v.VisitorIdentifierNIDorPassportNumber.Equals(visitorNIDorPassportNum), false).Any();

		public async Task CreateVisitorBlackList(VisitorBlackList visitorBlackList)
		=>await Create(visitorBlackList);

		public void DeleteVisitorFromBlackList(VisitorBlackList visitorBlackList)
		=> SoftDelete(visitorBlackList);

		public async Task<PagedList<VisitorBlackList>> GetAllVisitorsInBlackList(VisitorBlackListRequestParameters visitorBlackListRequestParameters, bool trackChanges)
		{
			var data = await FindAll(trackChanges).Search(visitorBlackListRequestParameters.SearchTerm).ToListAsync();
			return PagedList<VisitorBlackList>.ToPagedList(data, visitorBlackListRequestParameters.PageNumber, visitorBlackListRequestParameters.PageSize);
		}
		public async Task<VisitorBlackList> GetVisitorBlackListById(int id, bool trackChanges)
		=>await FindByCondition(v => v.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

		public async Task<VisitorBlackList?> GetVisitorBlackListByNID(string Nid, bool trackChanges)
		=> await FindByCondition(v => v.VisitorIdentifierNIDorPassportNumber.Equals(Nid), trackChanges).SingleOrDefaultAsync(); 
	}
}