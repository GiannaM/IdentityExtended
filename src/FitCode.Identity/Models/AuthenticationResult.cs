namespace FitCode.Identity.Models
{
    public sealed class AuthenticationResult<T, TP> where T : IUserEntity<TP> where TP : struct
    {
        public AuthenticationResultStatus Status { get; private set; }
        public T User { get; private set; }

        internal AuthenticationResult(T user, AuthenticationResultStatus status)
        {
            User = user;
            Status = status;
        }

        internal static AuthenticationResult<UserIdentity, long> Error => new AuthenticationResult<UserIdentity, long>(null, AuthenticationResultStatus.Error);
        internal static AuthenticationResult<UserIdentity, long> NotFound => new AuthenticationResult<UserIdentity, long>(null, AuthenticationResultStatus.NotFound);
        internal static AuthenticationResult<UserIdentity, long> MigrationNeeded => new AuthenticationResult<UserIdentity, long>(null, AuthenticationResultStatus.MigrationNeeded);
    }
}
