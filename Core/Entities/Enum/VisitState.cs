namespace Core.Entities.Enum
{
    public enum VisitState:byte
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
    public enum VisitType:byte
	{
		Employee = 0,
		External = 1
	}
	public enum CardState:byte
    {
        Available = 0,
        NotAvailable = 1
    }
    public enum VisitorIdentifierType : byte
	{
		NID = 0,
		Passport = 1
	}



}
