using System;
namespace TransactionManager.Models.APIModels.Login
{
	public class TokenBase
	{
        public string access_token { get; set; }
        public DateTime expires_at { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public TokenBase(string accessToken, string type, string refreshToken, DateTime expiresAt)
        {
            this.access_token = accessToken;
            this.token_type = type;
            this.refresh_token = refreshToken;
            this.expires_at = expiresAt;
        }
        protected TokenBase()
		{
		}
	}
}

