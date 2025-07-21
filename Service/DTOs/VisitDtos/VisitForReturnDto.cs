using Core.Entities.Enum;
using Service.DTOs.Visitor;

namespace Service.DTOs.Visit
{
	public record VisitForReturnDto
    {
        public long Id { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime PrimaryDate { get; set; }
        public DateTime? SecondaryDate { get; set; }
        public bool? IsPraimaryDateAccepted { get; set; }
        public string EmployeeNameInVisit { get; set; }
        public string Reason { get; set; }
        public string? Notes { get; set; }
        public VisitState VisitStateFromPolice { get; set; }
        public VisitState VisitStateFromDept { get; set; }
        public bool IsCreatedByDept { get; set; }
        public VisitType VisitType { get; set; }
		public string? ReasonforRejection { get; set; }
		public int FloorId { get; set; }
        public ICollection<VisitorForReturnDto> Visitors { get; set; }
    }
}
