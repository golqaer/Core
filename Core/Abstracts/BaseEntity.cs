using System.ComponentModel.DataAnnotations;

namespace Database.Abstracts
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        public long Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
