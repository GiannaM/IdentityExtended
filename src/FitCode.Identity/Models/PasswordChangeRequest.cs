namespace FitCode.Identity.Models
{
    public sealed class PasswordChangeRequest
    {
        public string Password { get; private set; }
        public string PasswordSha256 { get; private set; }
        public bool MustChangePassword { get; private set; }

        public PasswordChangeRequest(string password, string passwordSha256, bool mustChange)
        {
            Password = password;
            PasswordSha256 = passwordSha256;
            MustChangePassword = mustChange;
        }
    }
}
