using Core.Entities;
using Core.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Repository
{
    public class VisitorRepo : BaseRepository<Visitor>, IVisitorRepo
    {
        public VisitorRepo(FoeVisitContext context) : base(context)
        {
        }

        public void AssignCardToVisitor(Visitor visitor, long CardId)
        => visitor.CardId = CardId;

        public async Task CreateVisitor(long visitId, Visitor visitor)
        {
            visitor.VisitId = visitId;
            await base.Create(visitor);
        }

        public async Task<IEnumerable<Visitor>> GetAllVisitors(bool trackchanges)
        => await FindAll(trackchanges).ToListAsync();

        public async Task<Visitor> GetVisitorById(long visitorId, bool trackChanges)
        => await FindByCondition(v => v.Id.Equals(visitorId), trackChanges).SingleOrDefaultAsync();


		public IQueryable<Visitor>GetVisitorsInVisit(long visitId, bool trackchanges)
       => FindByCondition(v => v.VisitId.Equals(visitId), trackchanges);
            
        public void RegisterEntryTime(Visitor visitor)
        {
			visitor.EntryTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
        }
        public void RegisterLeaveTime(Visitor visitor)
        {
            visitor.LeaveTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
        }

        
        // visitor BlackList
		public async Task<IEnumerable<Visitor>> GetVisitorInstancesByNID(string NID, bool trackChanges)
		=>await FindByCondition(v => v.NID.Equals(NID.Trim()) , trackChanges).ToListAsync();

		public void BlockVisitor(Visitor visitor)
		=> visitor.IsBloacked = true;

		public void UnBlockVisitor(Visitor visitor)
		=>visitor.IsBloacked = false;

        //public void BlockAndUnBlockVisitorInAllInstancesusingSql(string nid,bool isBlocked)
        //=> context.Database.ExecuteSqlInterpolated($"update [dbo].[Visitors] set IsBloacked={isBlocked} where NID='{nid}'");

        //public async Task<IEnumerable<Visitor>> GetLatestVisitorRecordByNid(string nid, bool trackchanges)
        //=> await FindByCondition(v => v.NID.Equals(nid) && v.EntryTime == null,trackchanges).ToListAsync();
    }
}