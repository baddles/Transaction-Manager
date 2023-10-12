using System;
namespace TransactionManager.Models.APIModels.Login
{
	// Path: /api/auth/initialize
	public class InitializeResponseModel
	{
		public string client_id { get; set; }
		public byte[] public_key { get; set; }
		private InitializeResponseModel()
		{
			
		}
		public InitializeResponseModel(string clientID, byte[] pubKey)
		{
			this.client_id = clientID;
			this.public_key = pubKey;
		}
	}

    // Path: /api/auth/login
    public class LoginResponseModel: TokenBase
	{
		public string current_timezone { get; set; }
        private LoginResponseModel(): base()
        {

        }
        public LoginResponseModel(string accessToken, string type, string refreshToken, DateTime expiresAt, string currentTimezone) : base(accessToken, type, refreshToken, expiresAt)
        {
			this.current_timezone = currentTimezone;
        }
    }

}

