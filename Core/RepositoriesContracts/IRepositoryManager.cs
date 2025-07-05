using Core.RepositoriesContracts;
using Microsoft.AspNetCore.Http;

namespace Core.RepositoryContracts
{
	public interface IRepositoryManager
    {
        public IUserRepo UserRepo { get; }
        public ICardRepo CardRepo { get; }
        public IFloorRepo FloorRepo{ get; }
        public IVisitRepo VisitRepo { get;}
        public IVisitorRepo VisitorRepo { get;}
        public IDepartmentRepo DepartmentRepo { get; }
		public IVisitorBlackListRepo BlackListRepo { get; }
		public IStorageService StorageService { get; }

		public Task SaveAsync();
    }
	public interface IStorageService
	{
		Task<string> SaveVisitorNidAsync(IFormFile file, string departmentName, DateTime visitDate);
	}
	

}
