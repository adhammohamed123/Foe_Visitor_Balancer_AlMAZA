using Core.Entities;
using System.Linq.Expressions;

namespace Core.RepositoriesContracts
{
    public interface IFloorRepo
    {

        IQueryable<Floor> GetAllFloorList(bool trackchanges);
        IQueryable<Floor> GetFloorListBasedOnCondition(Expression<Func<Floor,bool>> expression,bool trackChanges);
        public Task<Floor> GetFloorById(long Id, bool trackchanges);

        Task Create(Floor floor,string userId);
        public void UpdateFloor(Floor floor,string userId);

        public void DeleteFloor(Floor floor,string userId);
		public bool CheckFloorExistsWithSameName(string name);


	}
}
