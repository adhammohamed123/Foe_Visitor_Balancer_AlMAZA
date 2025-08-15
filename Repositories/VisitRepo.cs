using Core.Entities;
using Core.Features;
using Core.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repository.Extensions;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public class VisitRepo : BaseRepository<Visit>, IVisitRepo
    {

        public VisitRepo(FoeVisitContext context) : base(context)
        {

        }

        public async Task CreateNewVisit(Visit visit, string userId)
        {
            visit.CreatedUserId = userId;
            await Create(visit);
        }

        public IQueryable<Visit> FindVisitsInFloor(long floorId, bool trackchanges)
        => base.FindByCondition(v => v.FloorId.Equals(floorId), trackchanges);

        public PagedList<Visit> GetAllVisits(VisitRequestParameters visitRequestParameters, bool trackchanges)
        {
            var data = FindByCondition(v=>v.Visitors.Count()>0,trackchanges)
			 .Include(v => v.Visitors).ThenInclude(v=>v.Card).Include(v => v.CreatedUser).ThenInclude(u => u.Department).AsSplitQuery()
			 .filter(visitRequestParameters.FloarId, 
             visitRequestParameters.VisitStateFromPolice,
             visitStateFromDept:visitRequestParameters.VisitStateFromDept
             ,visitRequestParameters.VisitType,
             visitRequestParameters.IsCreatedByDept,visitRequestParameters.CardId)
             .Search(visitRequestParameters.SearchTerm)
             .Sort(visitRequestParameters.OrderBy)
             .ToList();
            return PagedList<Visit>.ToPagedList(data, visitRequestParameters.PageNumber, visitRequestParameters.PageSize);
        }
        public PagedList<Visit> GetAllVisitsInThisDay(VisitRequestParameters visitRequestParameters)
        {
            var today = DateTime.UtcNow.Date;
            var data = FindByCondition(v => v.VisitDate >= today && v.VisitDate < today.AddDays(1) , false)
                     .Include(v => v.Visitors.Where(v=>v.CardId!=null || (v.CardId==null && v.IsBloacked==true))).ThenInclude(v => v.Card)
                     .Include(v => v.CreatedUser).ThenInclude(u => u.Department).AsSplitQuery()
					.filter(visitRequestParameters.FloarId, 
                    visitStateFromPolice: Core.Entities.Enum.VisitState.Approved,
                    visitStateFromDept: null,
                    visitRequestParameters.VisitType,
                    isCreatedByDept: null,null)
                      .Search(visitRequestParameters.SearchTerm)
                      .Sort(visitRequestParameters.OrderBy)
                     .ToList();

            return PagedList<Visit>.ToPagedList(data, visitRequestParameters.PageNumber, visitRequestParameters.PageSize);
        }

        public async Task<Visit> GetVisitById(long visitId, bool trackchanges)
        => await FindByCondition(v => v.Id.Equals(visitId), trackchanges).SingleOrDefaultAsync();

        public async Task<Visit> GetVisitDetailsWithVisitors(long visitId, bool trackchanges)
        => await FindByCondition(v => v.Id.Equals(visitId), trackchanges).Include(v => v.Visitors).SingleOrDefaultAsync();

        public PagedList<Visit> GetVisitsForUser(VisitRequestParameters visitRequestParameters, string UserId, bool trackchanges)
        {
          var data=  FindByCondition(v => v.CreatedUserId.Equals(UserId), trackchanges)
            .Include(v => v.Visitors).ThenInclude(v => v.Card)
            .filter(visitRequestParameters.FloarId, 
            visitStateFromPolice: visitRequestParameters.VisitStateFromPolice,
            visitStateFromDept:visitRequestParameters.VisitStateFromDept,
            visitRequestParameters.VisitType,
            isCreatedByDept: visitRequestParameters.IsCreatedByDept,null)
            .Search(visitRequestParameters.SearchTerm)
            .Sort(visitRequestParameters.OrderBy)
            .ToList();

            return PagedList<Visit>.ToPagedList(data, visitRequestParameters.PageNumber, visitRequestParameters.PageSize);
        }
    }
}