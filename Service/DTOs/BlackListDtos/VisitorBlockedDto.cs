using Core.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.BlackListDtos
{
    public record VisitorBlockedDto
	{
		public	int Id { get; set; }
		public string VisitorIdentifierNIDorPassportNumber { get; set; }
        public VisitorIdentifierType VisitorIdentifierType { get; set; }
        public string ReasonForBlocking { get; set; }
    }
   
    public record VisitorBlockedForCreationDto
    {
        
        public string VisitorIdentifierNIDorPassportNumber { get; set; }
        public VisitorIdentifierType VisitorIdentifierType { get; set; }
        public string ReasonForBlocking { get; set; }
        
    }
}
