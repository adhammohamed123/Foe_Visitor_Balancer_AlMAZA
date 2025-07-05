using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.UserDtos
{
    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string? UserName { get; init; }
        [Required(ErrorMessage = "Password name is required")]
        public string? Password { get; init; }
    }

    public record UserDto
    {
		public string Id { get; set; }
		public string DepartmentId { get; set; }
        public string Name { get; set; }
    }
    
}
