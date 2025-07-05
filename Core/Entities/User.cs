using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;
using Core.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class User:IdentityUser,ISoftDeletedModel
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsDeleted { get ; set ; }
        [Key,ForeignKey(nameof(Department))]
        [MaxLength(64)]
        public string DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Visit> Visits { get; set; } = new HashSet<Visit>();
	}

}
