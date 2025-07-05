using Core.Contracts;

namespace Core.Entities
{
	public class VisitorBlackList : ISoftDeletedModel
	{
		public int Id { get; set; }
		public  string VisitorIdentifierNIDorPassportNumber { get; set; }
		public bool  IsDeleted { get; set; }
	}

}
