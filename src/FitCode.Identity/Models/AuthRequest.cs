namespace FitCode.Identity.Models
{
    public sealed class AuthRequest
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public AuthRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
