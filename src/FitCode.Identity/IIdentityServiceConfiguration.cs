using System;
using System.Configuration;
using FitCode.Identity.Extensions;

namespace FitCode.Identity
{
    public interface IIdentityServiceConfiguration
    {
        bool AutomaticRehash { get; }
        bool EnabledUserLockout { get; }
        bool SilentExceptions { get; }
        TimeSpan DefaultLockoutDuration { get; }
        short MaxRetry { get; }
        int CooldownPeriodInMinutes { get; }
    }

    public sealed class DefaultIdentityServiceConfiguration : IIdentityServiceConfiguration
    {
        public bool SilentExceptions => ConfigurationManager.AppSettings.GetFromCollection<bool>("Identity.SilentExceptions", true);
        public bool AutomaticRehash => ConfigurationManager.AppSettings.GetFromCollection<bool>("Identity.AutomaticRehash", true);
        public bool EnabledUserLockout => ConfigurationManager.AppSettings.GetFromCollection<bool>("Identity.EnabledUserLockout", true);
        public short MaxRetry => ConfigurationManager.AppSettings.GetFromCollection<short>("Identity.MaxRetry", 5);
        public int CooldownPeriodInMinutes => ConfigurationManager.AppSettings.GetFromCollection<short>("Identity.CooldownPeriodInMinutes", 10);

        public TimeSpan DefaultLockoutDuration
        {
            get
            {
                var defaultLockoutDuration = ConfigurationManager.AppSettings.GetFromCollection<string>("Identity.DefaultLockoutDuration");
                if (!string.IsNullOrEmpty(defaultLockoutDuration))
                    if (TimeSpan.TryParse(defaultLockoutDuration, out TimeSpan duration))
                        return duration;

                return TimeSpan.FromMinutes(5);
            }
        }
    }
}
