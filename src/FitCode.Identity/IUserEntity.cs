using System;

namespace FitCode.Identity
{
    public interface IUserEntity<T> where T : struct
    {
        T Id { get; }
        string UserName { get; }
        string Password { get; }
        short AccessFailedCount { get; }
        DateTimeOffset? LockoutEnd { get; }
    }
}
