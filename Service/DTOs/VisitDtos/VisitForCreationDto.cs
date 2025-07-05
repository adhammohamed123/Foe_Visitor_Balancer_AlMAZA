using Core.Entities.Enum;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.Visit
{
    public record VisitForCreationDto
    {
        public DateTime VisitDate { get; set; }
        public string EmployeeNameInVisit { get; set; }
        public string Reason { get; set; }
		public VisitType VisitType { get; set; }
		public string? Notes { get; set; }
        public int FloorId { get; set; }
    }

    public record VisitForCreationFromSecertaryDto : VisitForCreationDto
    {
        [MaxLength(64)]
        public string CreatedUserId { get; set; }
    }
}
