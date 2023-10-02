using System;
using TransactionManager.Models.APIModels.Login.Authorization.TokenResponseModel;
using TransactionManager.Models.DBModels;
using TransactionManager.Service.Login.Authorization;
using TransactionManager.Models.APIModels.Login;

namespace TransactionManager.Facade.Login
{
	public static class AuthorizationFacade
	{
		private static ImgurAPIResponseModel getImgurToken(string code, IConfiguration usersettingsconfig)
		{
			string client_id = usersettingsconfig["imgur.com:client_id"];
            string client_secret = "";
            if (string.IsNullOrEmpty(client_secret = usersettingsconfig["imgur.com:client_secret"]))
            {
                throw new InvalidOperationException("imgur.com client_secret not initialized/found!");
            }
			ImgurOauth2Service imgurAuthenticationService = new ImgurOauth2Service(usersettingsconfig["imgur.com:api_path"], client_id, client_secret, usersettingsconfig["imgur.com:grant_type"]);
			return imgurAuthenticationService.generateAccessToken(code);
		}
		private static TMTokenResponseModel getTransactionManagerToken(string username, DateTime execution_time, DatabaseContext dbContext, IConfiguration usersettingsconfig)
		{
			TransactionManagerTokenService tmService = new TransactionManagerTokenService(dbContext, usersettingsconfig);
			return tmService.generateToken(username,execution_time, Guid.NewGuid());
		}
		public static LoginResponseModel getAuthorizationToken(string username, string code, DatabaseContext _dbContext, IConfiguration _config)
		{
			DateTime exec_time = DateTime.UtcNow;
			LoginResponseModel responseModel = new LoginResponseModel(getImgurToken(code, _config), getTransactionManagerToken(username, exec_time, _dbContext, _config));
			return responseModel;
		}
	}
}

