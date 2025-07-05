namespace Service.DTOs.Visitor
{
	public record VisitorTakeCardInSpecifecVisitDto
	{
		public long Id { get; set; }
		public long VisitId { get; set; }
		public long CardId { get; set; }
	}

    

}
