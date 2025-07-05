namespace Service.DTOs.BlackListDtos
{
    public record VisitorBlockedDto
	{
		public	int Id { get; set; }
		public string VisitorIdentifierNIDorPassportNumber { get; set; }
	}
   
}
