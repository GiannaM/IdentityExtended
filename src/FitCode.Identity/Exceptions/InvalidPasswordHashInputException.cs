using System;

namespace FitCode.Identity.Exceptions
{
    public sealed class InvalidPasswordHashInputException : BaseException
    {
        public InvalidPasswordHashInputException() : base(ExceptionErrorCode.Identity_InvalidPasswordHashInput, "Invalid Password Hash Input")
        {

        }

        public InvalidPasswordHashInputException(Exception ex) : base(ExceptionErrorCode.Identity_InvalidPasswordHashInput, "Invalid Password Hash Input", ex)
        {

        }
    }
}
