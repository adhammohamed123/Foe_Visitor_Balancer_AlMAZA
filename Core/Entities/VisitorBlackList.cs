using Core.Contracts;
using Core.Entities.Enum;

namespace Core.Entities
{
	public class VisitorBlackList : ISoftDeletedModel
	{
		public int Id { get; set; }
		public  string VisitorIdentifierNIDorPassportNumber { get; set; }
        public VisitorIdentifierType VisitorIdentifierType { get; set; }
        public string ReasonForBlocking { get; set; }
        public bool  IsDeleted { get; set; }
	}

}
