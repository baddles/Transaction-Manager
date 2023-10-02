using System;
namespace TransactionManager.Models.APIModels.Login.Authorization.TokenResponseModel
{
	public class TokenBase<T>
	{
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public T expiration_time { get; set; }
        public TokenBase(string access_token, string token_type, string refresh_token, T expiration_time = default(T))
		{
			this.access_token = access_token;
			this.token_type = token_type;
			this.refresh_token = refresh_token;
			this.expiration_time = expiration_time;
		}

	}
}

