namespace Domain.IdentityBaseModels
{
    public class UserRole : UserRole<string>
    {
    }

    public class UserRole<TKey>
    {
        public TKey UserId { get; set; }
        public virtual User User { get; set; }

        public TKey RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
