using System.Threading;
using System.Threading.Tasks;

namespace FitCode.Identity
{
    public interface IUserRepository<T, PT> where T : IUserEntity<PT>
        where PT : struct
    {
        Task<T> FindByUsernameAsync(string username, CancellationToken cancellationToken = default(CancellationToken));
        T FindByUsername(string username);

        Task<T> GetByIdAsync(PT id, CancellationToken cancellationToken = default(CancellationToken));
        T GetById(PT id);

        void UnitOfWorkCommit();
    }
}
