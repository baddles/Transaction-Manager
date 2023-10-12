using System;
using Org.BouncyCastle.Asn1.Ocsp;
using TransactionManager.Models.APIModels.Login;
using TransactionManager.Response;
using TransactionManager.Service.Login.Authentication;
using TransactionManager.Models.DBModels;
namespace TransactionManager.Facade.Login
{
	public static class AuthenticationFacade
	{
		public static InitializeResponseModel Initialize(string state, IConfiguration usersettingsconfig)
		{
            string? client_id = null;
            if (string.IsNullOrEmpty(client_id = usersettingsconfig["imgur.com:client_id"]))
            {
                throw new InvalidOperationException("imgur.com Client_ID not initialized/found!");
            }
            byte[] public_key = KeyManagementService.initializeInstance(state);
            InitializeResponseModel responseModel = new InitializeResponseModel(client_id, public_key);
			return responseModel;
        }
        public static bool CheckPassword(DatabaseContext dbContext, string state, string username, string password)
        {
            LoginService loginService = new LoginService(dbContext);
            string? hashedPassword = loginService.getHashedPassword(username);
            RSAService rsa = new RSAService();
            var privateKey = System.Text.Encoding.UTF8.GetString(KeyManagementService.getPrivateKey(state));
            if (string.IsNullOrEmpty(hashedPassword) || !BCrypt.Net.BCrypt.Verify(rsa.DecryptMessage(Convert.FromBase64String(password), KeyManagementService.getPrivateKey(state)), hashedPassword.Replace("\0", string.Empty)))
            {
                return false;
            }
            KeyManagementService.RemoveKey(state);
            return true;
        }
	}
}

