using Core.Entities;
using Core.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Threading.Tasks;

public class DepartmentRepo : BaseRepository<Department>, IDepartmentRepo
{
    public DepartmentRepo(FoeVisitContext context) : base(context)
    {
    }

    public bool ChackExistanceDeptWithTheSameName(string deptName)
    => FindByCondition(d => d.Name.ToLower().Equals(deptName.ToLower().Trim()), false).Any();

    public async Task CreateDepartmentAsync(Department department)
   =>await Create(department);

    public void DeleteDepartment(Department department)
     => SoftDelete(department);

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(bool trackChanges)
    =>await FindAll(trackChanges).ToListAsync();

    public async Task<Department> GetDepartmentByIdAsync(string id, bool trackChanges)
    =>await FindByCondition(d => d.Id.Equals(id.Trim()), trackChanges).SingleOrDefaultAsync();

    public  void UpdateDepartment(Department department)
    =>Update(department);
}