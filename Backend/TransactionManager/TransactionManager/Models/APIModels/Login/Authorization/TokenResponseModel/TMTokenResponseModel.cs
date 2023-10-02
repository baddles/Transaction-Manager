using System;
namespace TransactionManager.Models.APIModels.Login.Authorization.TokenResponseModel
{
	public class TMTokenResponseModel : TokenBase<DateTime>
	{
		public TMTokenResponseModel(string access_token, string token_type, string refresh_token, DateTime expires_at) : base(access_token, token_type, refresh_token, expires_at)
        {
		}
	}
}

