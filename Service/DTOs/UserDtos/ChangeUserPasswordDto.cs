using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.UserDtos
{
    public record ChangeUserPasswordDto
    {
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public string UserId { get; set; }
      
        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        public string NewPassword { get; init; }
        [Compare(nameof(NewPassword), ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPass { get; set; }
    }
}
