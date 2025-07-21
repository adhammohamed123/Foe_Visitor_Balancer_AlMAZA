using Service.DTOs.FloorDtos;

namespace Service.Service
{
    public interface IFloorService
    {
        Task<FloorForReturnDto> CreateFloor(FloorForCreationDto forCreationDto, string userId);
        IEnumerable<FloorForReturnDto> GetAllFloor(bool trackchanges);
        Task<FloorForReturnDto> GetFloorById(long Id, bool trackChanges);
        Task DeleteAsync(long id, string userId);
    }
}
