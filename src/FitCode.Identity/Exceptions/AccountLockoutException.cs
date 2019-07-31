using FitCode.Identity.Models;

namespace FitCode.Identity.Exceptions
{
    public sealed class AccountLockoutException<T, TP> : BaseException where T : IUserEntity<TP> where TP : struct
    {
        public T User { get; }

        public AccountLockoutException(T userAuthEntity) : base(ExceptionErrorCode.Identity_Lockout)
        {
            User = userAuthEntity;
        }

        public AuthenticationResult<T, TP> GetAuthenticationResult()
        {
            return new AuthenticationResult<T,TP>(User, AuthenticationResultStatus.Lockout);
        }
    }
}
