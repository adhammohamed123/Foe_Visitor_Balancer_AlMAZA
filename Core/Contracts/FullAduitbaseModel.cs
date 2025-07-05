using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Contracts
{
    public abstract class FullAduitbaseModel : IAuditedModel, ISoftDeletedModel
    {
        [ForeignKey(nameof(CreatedUser)), MaxLength(64)]
        public string CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey(nameof(LastModifiedUser)), MaxLength(64)]
        public string? LastModifiedUserId { get; set; }
        public User LastModifiedUser { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long Id { get; set; }


    }

}
