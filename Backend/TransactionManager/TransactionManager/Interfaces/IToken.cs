using System;
using TransactionManager.Models.DBModels.Tables;
namespace TransactionManager.Interfaces
{
	public interface IToken
	{
        public Microsoft.EntityFrameworkCore.DbSet<Token> getToken();
        public void setToken(Token t);
        public bool deleteToken(string username);
    }
}

