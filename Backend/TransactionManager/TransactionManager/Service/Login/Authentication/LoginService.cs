using System;
using TransactionManager.Models.DBModels;
using TransactionManager.Repository;
namespace TransactionManager.Service.Login.Authentication
{
	public class LoginService
	{
		private readonly DatabaseContext _dbContext;
		public LoginService(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
        public string? getHashedPassword(string username)
        {
			UserRepository userRepository = new UserRepository(_dbContext);
			byte[]? hashedPW = userRepository.GetUserPasswordFromDB(username);
			if (hashedPW == null)
			{
				return null;
			}
			return System.Text.Encoding.UTF8.GetString(hashedPW);
        }
    }
	
}

