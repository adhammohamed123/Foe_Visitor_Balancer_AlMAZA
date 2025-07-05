namespace Core.Contracts
{
    public interface ISoftDeletedModel
    {
        public bool IsDeleted { get; set; }
    }

}
