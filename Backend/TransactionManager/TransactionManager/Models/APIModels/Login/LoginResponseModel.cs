using System;
using TransactionManager.Models.APIModels.Login.Authorization.TokenResponseModel;
namespace TransactionManager.Models.APIModels.Login
{
	// Path: /api/auth/initialize
	public class InitializeResponseModel
	{
		public string client_id { get; set; }
		public byte[] public_key { get; set; }
	}

    // Path: /api/auth/login
    public class LoginResponseModel
	{
		public ImgurAPIResponseModel imgurOAuth2Token { get; set; }
		public TMTokenResponseModel TMToken { get; set; }
		public LoginResponseModel(ImgurAPIResponseModel imgurTokenData, TMTokenResponseModel TransactionManagerTokenData)
		{
			this.imgurOAuth2Token = imgurTokenData;
			this.TMToken = TransactionManagerTokenData;
		}
	}

}

