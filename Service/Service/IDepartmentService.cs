using Service.DTOs.DepartmentDtos;

namespace Service.Service
{
	public interface IDepartmentService
    {
        Task<DepartmentForReturnDto> CreateNewDepartment(DepartmentForCreationDto forCreationDto);
        Task<IEnumerable<DepartmentForReturnDto>> GetAllDepartments(bool trackchanges);
        Task UpdateDepartment(DepartmentForReturnDto departmentDto);
        Task DeleteDepartment(string DeptId);

    }
}
