using System;
using TransactionManager.Interfaces;
using TransactionManager.Models.DBModels;
namespace TransactionManager.Repository
{
	public class UserRepository : IUser
	{
		readonly DatabaseContext _dbContext;
		public UserRepository(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public byte[]? GetUserPasswordFromDB(string username)
        {
			return _dbContext.user.Where(user => user.username == username)
                                             .Select(user => user.hashedPassword)
                                             .FirstOrDefault();
        }
	}
}

