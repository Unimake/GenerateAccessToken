namespace Unimake.GenerateAccessToken.Models
{
    public class AuthResponse
    {
        #region Public Properties

        public long Expiration { get; init; }

        public string Token { get; init; }

        public string Type { get; init; }

        #endregion Public Properties
    }
}