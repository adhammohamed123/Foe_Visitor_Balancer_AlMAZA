using Core.Entities.Enum;

namespace Service.DTOs.Visit
{
	public record VisitStatusChangeFromPoliceDto
    {
        public long Id { get; set; }
        public VisitState VisitStateFromPolice { get; set; }
        public string? ReasonforRejection { get; set; }
    }
    public record VisitStatusChangeFromDeptDto
    {
        public long Id { get; set; }
        public VisitState VisitStateFromDept { get; set; }
    }
}
