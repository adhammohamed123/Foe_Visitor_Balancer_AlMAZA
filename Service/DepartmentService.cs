using AutoMapper;
using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.RepositoryContracts;
using Service.DTOs.DepartmentDtos;
using Service.Service;
using System.Threading.Tasks;
using static Core.Exceptions.BadRequestException;

namespace Service
{
    public class DepartmentService : IDepartmentService
    {
        private IRepositoryManager repositoryManager;
        private IMapper mapper;
        private ILoggerManager logger;

        public DepartmentService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger)
        {
            this.repositoryManager = repositoryManager;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<DepartmentForReturnDto> CreateNewDepartment(DepartmentForCreationDto forCreationDto)
        {
            if (repositoryManager.DepartmentRepo.ChackExistanceDeptWithTheSameName(forCreationDto.Name))
                throw new DepartmentBadRequestException(forCreationDto.Name);
            var dept = mapper.Map<Department>(forCreationDto);
            await repositoryManager.DepartmentRepo.CreateDepartmentAsync(dept);
            await repositoryManager.SaveAsync();
            return mapper.Map<DepartmentForReturnDto>(dept);
        }

		public async Task DeleteDepartment(string DeptId)
		{
			var dept=await CheckExistance(DeptId, true);
            var user=await repositoryManager.UserRepo.GetFromUserById(dept.Id, true);
            if (user != null)
                throw new DepartmentDeleteBadRequestException(dept.Id);
			repositoryManager.DepartmentRepo.DeleteDepartment(dept);
            //repositoryManager.UserRepo.DeleteUser(user);
			await repositoryManager.SaveAsync();
		}

		public async Task<IEnumerable<DepartmentForReturnDto>> GetAllDepartments(bool trackchanges)
        {
            var allDepts = await repositoryManager.DepartmentRepo.GetAllDepartmentsAsync(trackchanges);
            return mapper.Map<IEnumerable<DepartmentForReturnDto>>(allDepts);
        }

		public async Task UpdateDepartment(DepartmentForReturnDto departmentDto)
		{
			var dept =await CheckExistance(departmentDto.Id, true);
            mapper.Map(departmentDto,dept);
            await repositoryManager.SaveAsync();
		}

		private async Task<Department> CheckExistance(string id,bool trackchanges)
        {
           var dept=  await  repositoryManager.DepartmentRepo.GetDepartmentByIdAsync(id, trackchanges);
            if (dept == null)
                throw new DepartmentNotFoundException(id);
            return dept;
        }
    }
}
