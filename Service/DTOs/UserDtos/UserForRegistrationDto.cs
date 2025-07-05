using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.UserDtos
{
    public record UserForRegistrationDto
    {
        public string DepartmentId { get; init; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string UserName { get; init; }
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
       
        [StringLength(100, MinimumLength = 5, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 5 أحرف")]
        public string Password { get; init; }
        [StringLength(11, MinimumLength = 11)]
        public string? PhoneNumber { get; init; }
        public string Role { get; init; }
    }

    public record UserForReturnDto
    {
		public string Id { get; set; }
		public string UserName { get; init; }
        [StringLength(11, MinimumLength = 11)]
        public string? PhoneNumber { get; init; }
        public string DepartmentName { get; set; }

		public string  Role { get; set; }
	}
}
