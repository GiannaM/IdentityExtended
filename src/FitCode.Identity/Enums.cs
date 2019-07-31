using System;

namespace FitCode.Identity
{
    [Serializable]
    public enum PasswordVerificationResultStatus : ushort
    {
        Failed = 0,
        Success = 1,
        SuccessRehashNeeded = 2
    }

    [Serializable]
    public enum AuthenticationResultStatus : ushort
    {
        Valid = 0,
        InvalidPassword = 1,
        NotFound = 2,
        Error = 3,
        MigrationNeeded = 4,
        Lockout = 5
    }

    [Serializable]
    public enum ExceptionErrorCode : ushort
    {
        Identity_InvalidPassword = 1000,
        Identity_DatabaseHashMigration,
        Identity_Lockout,
        Identity_InvalidPasswordHashInput
    }
}
