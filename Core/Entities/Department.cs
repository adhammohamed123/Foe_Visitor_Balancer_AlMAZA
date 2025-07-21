using Core.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Department:ISoftDeletedModel
    {
        [MaxLength(64)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeptColor { get; set; }
        public bool IsDeleted { get; set; }
        public User UserAccount { get; set; }
        public Department()=>Id= Guid.NewGuid().ToString();
       

    }

}
