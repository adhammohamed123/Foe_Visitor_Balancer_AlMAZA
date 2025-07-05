using Service.Service;

namespace Service.Services
{
	public interface IServiceManager
    {
        public IFloorService FloorService { get; }
        public ICardService CardService { get; }
        public IVisitorService VisitorService { get; }
        public IVisitService VisitService { get; }
        public IUserService UserService { get; }
        public IDepartmentService DepartmentService { get; }
		public IVisitorBlackListService   VisitorBlackListService { get;}
		
	}
}
