using FitCode.Identity.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FitCode.Identity
{
    public interface IIdentityService<T, PT> : IIdentityLockoutService<T, PT> where T : IUserEntity<PT>
        where PT : struct
    {
        IExtendedPasswordHasher Hasher { get; }

        AuthenticationResult<T,PT> Auth(AuthRequest authRequest);
        Task<AuthenticationResult<T, PT>> AuthAsync(AuthRequest authRequest, CancellationToken cancellationToken = default(CancellationToken));

        void UpdateUserPassword(long userId, PasswordChangeRequest request);
    }
}
