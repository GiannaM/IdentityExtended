using System;

namespace FitCode.Identity.Models
{
    public class UserIdentity : IUserEntity<long>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public short AccessFailedCount { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
