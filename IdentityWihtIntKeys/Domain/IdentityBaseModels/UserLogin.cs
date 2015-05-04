namespace Domain.IdentityBaseModels
{
    /// <summary>
    ///     Entity type for a user's login (i.e. facebook, google)
    /// </summary>
    public class UserLogin : UserLogin<string>
    {
    }

    public class UserLogin<TKey>
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }

        public TKey UserId { get; set; }
        public virtual User User { get; set; }
    }
}
