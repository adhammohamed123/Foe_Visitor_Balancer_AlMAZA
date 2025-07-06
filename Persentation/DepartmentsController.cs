using Service.Services;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.DepartmentDtos;
using Presentaion.Attributes;
using Core.Entities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

[Route("api/Departments")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IServiceManager service;
    public DepartmentsController(IServiceManager service)
    {
        this.service = service;
    }
    [HttpPost]
    [Authorize(Roles = "nozom")]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Create([FromBody] DepartmentForCreationDto forCreationDto)
    {
        var dept =await service.DepartmentService.CreateNewDepartment(forCreationDto);
        var response = new ResponseShape<DepartmentForReturnDto>(StatusCodes.Status200OK, "Created Successfully", null, new List<DepartmentForReturnDto>() { dept });
        return Ok(response);
    }
    [HttpGet("getAll")]
	[Authorize(Roles = "nozom,secertary")]
	public  async Task<IActionResult> GetAll()
    {
        var data = await service.DepartmentService.GetAllDepartments(trackchanges: false);
        var response = new ResponseShape<DepartmentForReturnDto>(StatusCodes.Status200OK, "Ok", null,  data.ToList() );
        return Ok(response);
    }
    [HttpPut]
	[Authorize(Roles = "nozom")]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> Update([FromBody] DepartmentForReturnDto departmentDto)
	{
		await service.DepartmentService.UpdateDepartment(departmentDto);
		var response = new ResponseShape<DepartmentForReturnDto>(StatusCodes.Status200OK, "Updated Successfully", null, new List<DepartmentForReturnDto>() { departmentDto });
		return Ok(response);
	}
    [HttpDelete("{deptId}")]
	[Authorize(Roles = "nozom")]
	public async Task<IActionResult> Delete(string deptId)
	{
		await service.DepartmentService.DeleteDepartment(deptId);
		var response = new ResponseShape<object>(StatusCodes.Status200OK, "Deleted Successfully", null, null);
		return Ok(response);
	}
}
