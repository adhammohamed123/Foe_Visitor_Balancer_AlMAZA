using Core.Entities;

namespace Core.RepositoryContracts
{
    public interface IUserRepo
    {
	    Task<IEnumerable<User>> AllUser(bool trackchanges);
	    Task<User?> GetFromUserById(string id, bool trackchanges);
		Task<User?> GetFromUserByIdWithOutDepartment(string id, bool trackchanges);
		Task CreateUser(User User);  
        void DeleteUser(User User);
		Task<bool> ChackExistanceUserRelatedToSameDepartment(string deptId);

	}

}
