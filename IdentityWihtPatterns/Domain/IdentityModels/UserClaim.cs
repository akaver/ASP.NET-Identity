namespace Domain.IdentityModels
{
    /// <summary>
    ///     EntityType that represents one specific user claim
    /// </summary>
    public class UserClaim : UserClaim<string>
    {
    }

    public class UserClaim<TKey> 
    {
        public int Id { get; set; }

        public  TKey UserId { get; set; }
        public virtual User User { get; set; }

        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
    }
}
