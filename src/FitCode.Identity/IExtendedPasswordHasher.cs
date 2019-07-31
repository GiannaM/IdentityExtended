namespace FitCode.Identity
{
    public interface IExtendedPasswordHasher
    {
        string HashPassword(string password);

        PasswordVerificationResultStatus VerifyHashedPassword(string hashedPassword, string providedPassword);

        bool IsValidHashedPassword(string hashedPassword, string providedPassword);
    }
}
