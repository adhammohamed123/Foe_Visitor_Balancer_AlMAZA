using Core.Entities;
using Core.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Linq.Expressions;

namespace Repository
{
    public class FloorRepo:BaseRepository<Floor>,IFloorRepo
    {

        public FloorRepo(FoeVisitContext context):base(context)
        {
        }

		public bool CheckFloorExistsWithSameName(string name)
		=> FindByCondition(f => f.Name.ToLower().Equals(name.ToLower().Trim()), false).Any();
		

		public async Task Create(Floor floor, string userId)
        {
            floor.CreatedUserId = userId;  
            await base.Create(floor);
        }

        public void DeleteFloor(Floor floor, string userId)
        {
            floor.LastModifiedUserId=userId;
            base.SoftDelete(floor);
        }

        public IQueryable<Floor> GetAllFloorList(bool trackchanges)
        => FindAll(trackchanges);

        public async Task<Floor> GetFloorById(long Id,bool trackchanges)
        =>await FindByCondition(f => f.Id.Equals(Id), trackchanges).SingleOrDefaultAsync();

        public IQueryable<Floor> GetFloorListBasedOnCondition(Expression<Func<Floor, bool>> expression, bool trackChanges)
         => base.FindByCondition(expression, trackChanges);
        

        public void UpdateFloor(Floor floor, string userId)
        {
          floor.LastModifiedUserId = userId;
          base.Update(floor);
        }

       
    }
}