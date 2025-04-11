using Database.Abstracts;

namespace Database.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Admin { get; set; } = false;
    }
}
