using Core.Entities;

namespace Core.RepositoriesContracts
{
    public interface IDepartmentRepo
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(bool trackChanges);
        Task<Department> GetDepartmentByIdAsync(string id, bool trackChanges);
        Task CreateDepartmentAsync(Department department);
        void UpdateDepartment(Department department);
        void DeleteDepartment(Department department);
        bool ChackExistanceDeptWithTheSameName(string deptName);
    }
}

