using System;
namespace TransactionManager.Models.APIModels.Login
{
	public class RefreshTokenRequestModel
	{
		public string username { get; set; }
		public string refresh_token { get; set; }
	}
}

