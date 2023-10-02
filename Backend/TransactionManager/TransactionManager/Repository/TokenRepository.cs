using System;
using TransactionManager.Models.DBModels;
using TransactionManager.Interfaces;
using TransactionManager.Models.DBModels.Tables;
namespace TransactionManager.Repository
{
	public class TokenRepository : IToken
	{
		private readonly DatabaseContext _dbContext;
		public TokenRepository(DatabaseContext dbContext)
		{
			this._dbContext = dbContext;
		}
        public Microsoft.EntityFrameworkCore.DbSet<Token> getToken()
		{
			if (this._dbContext == null)
			{
				throw new Exception("Cannot connect to database");
			}
			return this._dbContext.token;
		}
		public void setToken(Token t)
		{
			this._dbContext.token.Add(t);
			this._dbContext.SaveChanges();
		}
		public bool deleteToken(string username)
		{
			var token = getToken().Where(u => u.username == username).FirstOrDefault();
            if (token == null)
			{
				return false;
			}
			this._dbContext.token.Remove(token);
			this._dbContext.SaveChanges();
			return true;
		}	
    }
}

