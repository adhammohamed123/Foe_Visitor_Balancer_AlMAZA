using Core.Entities;
using Core.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.RepositoriesContracts
{
    public interface IVisitRepo
    {
        public PagedList<Visit> GetAllVisits(VisitRequestParameters visitRequestParameters,bool trackchanges);
        Task Create(Visit visit);
        public Task CreateNewVisit(Visit visit, string userId);
        PagedList<Visit> GetAllVisitsInThisDay(VisitRequestParameters visitRequestParameters);
        IQueryable<Visit> FindVisitsInFloor(long floorId, bool trackchanges);
        Task<Visit> GetVisitDetailsWithVisitors(long visitId,bool trackchanges);
        PagedList<Visit> GetVisitsForUser(VisitRequestParameters visitRequestParameters,string UserId, bool trackchanges);
        Task<Visit>  GetVisitById(long visitId,bool trackchanges);
 

        

    }
}
