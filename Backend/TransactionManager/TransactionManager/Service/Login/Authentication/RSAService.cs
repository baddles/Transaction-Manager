using System;
using System.Security.Cryptography;
namespace TransactionManager.Service.Login.Authentication
{
	public class RSAService : IDisposable
	{
		private RSA rsa;
		public RSAService()
		{
			rsa = RSA.Create();
		}
		public byte[] ExportPrivateKey()
		{
			return rsa.ExportRSAPrivateKey();
		}
		public byte[] ExportPublicKey()
		{
			return rsa.ExportRSAPublicKey();
		}
		public string DecryptMessage(byte[] encryptedMessage, byte[] privateKeyBytes)
		{
			string test = System.Text.Encoding.UTF8.GetString(privateKeyBytes);
			rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

			byte[] decryptedBytes = rsa.Decrypt(encryptedMessage, RSAEncryptionPadding.Pkcs1);
			return System.Text.Encoding.UTF8.GetString(decryptedBytes);
		}
		public void Dispose()
		{
			rsa?.Dispose();
		}
	}
}

