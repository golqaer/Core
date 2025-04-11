namespace Database.Abstracts
{
    public interface IBaseEntity
    {
        long Id { get; set; }

        long LastUpdateTick { get; set; }

        bool IsDeleted { get; set; }
    }
}
