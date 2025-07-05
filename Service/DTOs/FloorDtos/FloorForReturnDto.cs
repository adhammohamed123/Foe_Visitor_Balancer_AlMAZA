namespace Service.DTOs.FloorDtos
{
	public record FloorForReturnDto
    {
        public string Color { get; set; }
        public string Name { get; set; }
		public string CardsFrom { get; set; }
		public string CardsTo { get; set; }
		public long Id { get; set; }

    }


}
