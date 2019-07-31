using FitCode.Identity.Exceptions;
using FitCode.Identity.Extensions;
using FitCode.Identity.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FitCode.Identity.Impl
{
    public sealed class IdentityService : IIdentityService<UserIdentity, long>
    {
        private readonly IIdentityServiceConfiguration _config;
        private readonly IUserRepository<UserIdentity, long> _userRepository;
        private readonly ILogger<IdentityService> _logger;

        public IExtendedPasswordHasher Hasher { get; }

        public IdentityService(IIdentityServiceConfiguration configuration,
            IUserRepository<UserIdentity, long> userRepository,
            IExtendedPasswordHasher passwordHasher, 
            ILogger<IdentityService> logger)
        {
            _config = configuration;
            _userRepository = userRepository;
            _logger = logger;

            Hasher = passwordHasher;
        }

        public AuthenticationResult<UserIdentity,long> Auth(AuthRequest authRequest)
        {
            long userId = -1;

            try
            {
                var result = _userRepository.FindByUsername(authRequest.Username);

                if (result == null) return AuthenticationResult<UserIdentity, long>.NotFound;
                else userId = result.Id;

                if (_config.EnabledUserLockout && IsUserLockout(result)) throw new AccountLockoutException<UserIdentity, long>(result);

                var passwordValidation = Hasher.VerifyHashedPassword(result.Password, authRequest.Password);
                switch (passwordValidation)
                {
                    case PasswordVerificationResultStatus.Success:
                    case PasswordVerificationResultStatus.SuccessRehashNeeded:
                        ResetAccessFailedCount(result);

                        if (passwordValidation == PasswordVerificationResultStatus.SuccessRehashNeeded && _config.AutomaticRehash)
                        {
                            result.Password = Hasher.HashPassword(authRequest.Password);
                        }

                        return new AuthenticationResult<UserIdentity, long>(result, AuthenticationResultStatus.Valid);
                    default:
                        IncrementAccessFailedCount(result);

                        return new AuthenticationResult<UserIdentity, long>(result, AuthenticationResultStatus.InvalidPassword);
                }
            }
            catch (AccountLockoutException<UserIdentity, long> lockoutEx)
            {
                return lockoutEx.GetAuthenticationResult();
            }
            catch (InvalidPasswordHashInputException invalidHashInputEx)
            {
                _logger.LogWarning("Invalid Password Hash", invalidHashInputEx);    
            }
            catch (Exception ex)
            {
                if (!_config.SilentExceptions) throw;

                _logger.LogError("Error Authenticating User", ex.WithState(new LoggerState
                {
                    Username = authRequest.Username
                }).AppendCorrelationId());
            }
            finally
            {
                _userRepository.UnitOfWorkCommit();
            }

            return AuthenticationResult<UserIdentity, long>.Error;
        }

        public void UpdateUserPassword(long userId, PasswordChangeRequest request)
        {
            var user = _userRepository.GetById(userId);

            if (user == null) throw new NullReferenceException("Unable to locate user");

            user.Password = request.Password;

            _userRepository.UnitOfWorkCommit();
        }

        public int IncrementAccessFailedCount(UserIdentity user)
        {
            user.AccessFailedCount += 1;

            if (user.AccessFailedCount >= _config.MaxRetry)
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(_config.CooldownPeriodInMinutes); // Timespan would be better for minute handling of minutes+seconds

                throw new AccountLockoutException<UserIdentity, long>(user);
            }

            return user.AccessFailedCount;
        }

        public bool IsUserLockout(UserIdentity user)
        {
            if (user.LockoutEnd.HasValue)
            {
                if (DateTimeOffset.UtcNow < user.LockoutEnd.Value) return true;
                if (user.AccessFailedCount >= _config.MaxRetry) user.AccessFailedCount = 0;
            }

            return false;
        }

        public void ResetAccessFailedCount(UserIdentity user)
        {
            user.AccessFailedCount = 0;
            user.LockoutEnd = null;
        }

        public async Task<AuthenticationResult<UserIdentity, long>> AuthAsync(AuthRequest authRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            long userId = -1;

            try
            {
                var result = await _userRepository.FindByUsernameAsync(authRequest.Username);

                if (result == null) return AuthenticationResult<UserIdentity, long>.NotFound;
                else userId = result.Id;

                if (_config.EnabledUserLockout && IsUserLockout(result)) throw new AccountLockoutException<UserIdentity, long>(result);

                var passwordValidation = Hasher.VerifyHashedPassword(result.Password, authRequest.Password);
                switch (passwordValidation)
                {
                    case PasswordVerificationResultStatus.Success:
                    case PasswordVerificationResultStatus.SuccessRehashNeeded:
                        ResetAccessFailedCount(result);

                        if (passwordValidation == PasswordVerificationResultStatus.SuccessRehashNeeded && _config.AutomaticRehash)
                        {
                            result.Password = Hasher.HashPassword(authRequest.Password);
                        }

                        return new AuthenticationResult<UserIdentity, long>(result, AuthenticationResultStatus.Valid);
                    default:
                        IncrementAccessFailedCount(result);

                        return new AuthenticationResult<UserIdentity, long>(result, AuthenticationResultStatus.InvalidPassword);
                }
            }
            catch (AccountLockoutException<UserIdentity, long> lockoutEx)
            {
                return lockoutEx.GetAuthenticationResult();
            }
            catch (InvalidPasswordHashInputException invalidHashInputEx)
            {
                _logger.LogWarning("Invalid Password Hash", invalidHashInputEx);
            }
            catch (Exception ex)
            {
                if (!_config.SilentExceptions) throw;

                _logger.LogError("Error Authenticating User", ex.WithState(new LoggerState
                {
                    Username = authRequest.Username
                }).AppendCorrelationId());
            }
            finally
            {
                _userRepository.UnitOfWorkCommit();
            }

            return AuthenticationResult<UserIdentity, long>.Error;
        }

        public Task<int> IncrementAccessFailedCountAsync(UserIdentity user, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserLockoutAsync(UserIdentity user, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(UserIdentity user, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
