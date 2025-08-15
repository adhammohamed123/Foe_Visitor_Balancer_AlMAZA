namespace Service.DTOs.Visitor
{
    public record VisitorForReturnDto
    {
        public  int  Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string NID { get; set; }
		//public string Notes { get; set; }
		public DateTime? EntryTime { get; set; }
		public DateTime? LeaveTime { get; set; }
		public string? NID_PicPath { get; set; }
		public long? CardId { get; set; }
		public bool IsBloacked { get; set; }
        public string? CardNumber { get; set; }

    }
   
}
