namespace FitCode.Identity.Exceptions
{
    public sealed class InvalidPasswordException : BaseException
    {
        public InvalidPasswordException() : base(ExceptionErrorCode.Identity_InvalidPassword, "Invalid Password")
        {

        }
    }
}
