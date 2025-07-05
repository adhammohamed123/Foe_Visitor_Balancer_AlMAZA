using Core.Contracts;
using Core.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Visitor :FullAduitbaseModel
    {
        public string Name { get; set; }
        public string  Phone { get; set; }
        public string  NID { get; set; }
        public string? NID_PicPath { get; set; }
		public bool IsBloacked { get; set; }
		public VisitorIdentifierType VisitorIdentifierType { get; set; }
		//public ICollection<VisitorInVisit> InVisits {  get; set; }
		#region Error In Mapping one to Many
		public DateTime? EntryTime { get; set; }
		public DateTime? LeaveTime { get; set; }
		//public string? Notes { get; set; }
		[ForeignKey(nameof(Visit))]
		public long VisitId { get; set; }
		public Visit Visit { get; set; }
		
		 [ForeignKey(nameof(Card))]
		public long? CardId { get; set; }
		public Card? Card { get; set; }
		#endregion

	}
	//public class VisitorInVisit:ISoftDeletedModel
	//{
	//	[ForeignKey(nameof(Visit))]
	//	public long VisitId { get; set; }
	//	public Visit Visit { get; set; }

	//	[ForeignKey(nameof(Visitor))]
	//	public long VisitorId { get; set; }
	//	public Visitor Visitor { get; set; }

	//	[ForeignKey(nameof(Card))]
	//	public long? CardId { get; set; }
	//	public Card? Card { get; set; }

	//	public DateTime? EntryTime { get; set; }
	//	public DateTime? LeaveTime { get; set; }
	//	public bool IsDeleted { get; set; }
	//}

}
