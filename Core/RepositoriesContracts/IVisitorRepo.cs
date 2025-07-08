using Core.Entities;

namespace Core.RepositoriesContracts
{
    public interface IVisitorRepo
    {
        Task CreateVisitor(long  visitId,Visitor visitor);
        Task<Visitor> GetVisitorById(long visitorId, bool trackChanges);
		Task<IEnumerable<Visitor>> GetAllVisitors(bool trackchanges);
        IQueryable<Visitor> GetVisitorsInVisit(long visitId,bool trackchanges);
        void AssignCardToVisitor(Visitor visitor, long CardId);
        void RegisterEntryTime(Visitor visitor);
        void RegisterLeaveTime(Visitor visitor);
		/// visitor BlackList
		Task<IEnumerable<Visitor>> GetVisitorInstancesByNID(string NID, bool trackChanges);
        void BlockVisitor(Visitor visitor);// old version
		void UnBlockVisitor(Visitor visitor);
      //  void BlockAndUnBlockVisitorInAllInstancesusingSql(string nid, bool isBlocked);//new version
        //Task<IEnumerable<Visitor>> GetLatestVisitorRecordByNid(string nid, bool trackchanges );
        
        
	}

    //public interface IVisitorInVisitRepo
    //{
    //    Task addVisitorToVisit(VisitorInVisit visitorInVisit);
        
    //}
}
