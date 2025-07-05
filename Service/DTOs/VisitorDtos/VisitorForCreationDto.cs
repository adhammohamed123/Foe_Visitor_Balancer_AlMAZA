using Core.Entities.Enum;
using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Visitor
{
    public record VisitorForCreationDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string NID { get; set; }
		//public string Notes { get; set; }
		public VisitorIdentifierType   VisitorIdentifierType { get; set; }
		public IFormFile? NID_PicPath { get; set; }
        
    }

    

}
