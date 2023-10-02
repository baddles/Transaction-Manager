using System;
namespace TransactionManager.Models.APIModels.Login
{
	public class LoginRequestModel
	{
        public string username { get; set; }
        public string encryptedPassword { get; set; } // Encrypted password using aesKey, hashed with SHA512(SHA256(password+md5(password)))
        public string code { get; set; }
        public string salt { get; set; }
	}
}

