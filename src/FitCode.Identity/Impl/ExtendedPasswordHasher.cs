using FitCode.Identity.Exceptions;
using Microsoft.AspNet.Identity;
using System;

namespace FitCode.Identity.Impl
{
    public sealed class ExtendedPasswordHasher : PasswordHasher, IExtendedPasswordHasher
    {
        public bool IsValidHashedPassword(string hashedPassword, string providedPassword)
        {
            var result = base.VerifyHashedPassword(hashedPassword, providedPassword);

            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }

        public new PasswordVerificationResultStatus VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            try
            {
                var result = (int)base.VerifyHashedPassword(hashedPassword, providedPassword);

                return (PasswordVerificationResultStatus)result;
            }
            catch (Exception ex) when (ex.Message.Contains("The input is not a valid Base-64 string as it contains a non-base 64 character"))
            {
                throw new InvalidPasswordHashInputException(ex);
            }
        }
    }
}
 