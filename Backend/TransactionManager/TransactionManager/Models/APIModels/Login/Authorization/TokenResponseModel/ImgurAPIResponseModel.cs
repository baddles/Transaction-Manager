using System;
namespace TransactionManager.Models.APIModels.Login.Authorization.TokenResponseModel
{
	public class ImgurAPIResponseModel : TokenBase<int>
	{
		public ImgurAPIResponseModel(string access_token, string token_type, string refresh_token, int expires_in) : base(access_token, token_type, refresh_token, expires_in)
		{
		}
	}
}

