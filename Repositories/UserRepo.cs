using Core.Entities;
using Core.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepo:BaseRepository<User>,IUserRepo
    {
       

        public UserRepo(FoeVisitContext context):base(context)
        {
            
        }

        public async Task CreateUser(User User)
       => await Create(User);

        public void DeleteUser(User User)
        => User.IsDeleted = true;

        public async Task<IEnumerable<User>> AllUser(bool trackchanges)
        => await FindAll(trackchanges).Include(u=>u.Department).ToListAsync();
        
        public async Task<User?> GetFromUserById(string id, bool trackchanges)
        => await FindByCondition(u => u.DepartmentId.Equals(id), trackchanges).Include(u=>u.Department).SingleOrDefaultAsync();

		public async Task<User?> GetFromUserByIdWithOutDepartment(string id, bool trackchanges)
		=> await FindByCondition(u => u.DepartmentId.Equals(id), trackchanges).SingleOrDefaultAsync();

		public async Task<bool> ChackExistanceUserRelatedToSameDepartment(string deptId)
		=>await FindByCondition(u => u.DepartmentId.Equals(deptId) , false).AnyAsync();
	}
}