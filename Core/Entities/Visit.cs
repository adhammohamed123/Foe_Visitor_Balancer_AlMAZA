using Core.Contracts;
using Core.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Visit :FullAduitbaseModel
    {
        public DateTime VisitDate { get; set; }
        public string EmployeeNameInVisit { get; set; }
        public string Reason{ get; set; }
        public string? Notes { get; set; }
        public VisitState VisitStateFromPolice { get; set; }=VisitState.Pending;
        public VisitState VisitStateFromDept { get; set; } = VisitState.Pending;
        public bool IsCreatedByDept { get; set; }
        public VisitType VisitType { get; set; } 
		public string? ReasonforRejection  { get; set; }
        [ForeignKey(nameof(Floor))]
		public long FloorId { get; set; }
        public Floor Floor { get; set; }
        public ICollection<Visitor> Visitors { get; set; }

    }

}
