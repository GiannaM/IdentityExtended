using System.Threading;
using System.Threading.Tasks;

namespace FitCode.Identity
{
    public interface IIdentityLockoutService<T, PT> where T : IUserEntity<PT>
        where PT : struct
    {
        int IncrementAccessFailedCount(T user);
        bool IsUserLockout(T user);
        void ResetAccessFailedCount(T user);

        Task<int> IncrementAccessFailedCountAsync(T user, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> IsUserLockoutAsync(T user, CancellationToken cancellationToken = default(CancellationToken));
        Task ResetAccessFailedCountAsync(T user, CancellationToken cancellationToken = default(CancellationToken));
    }
}
