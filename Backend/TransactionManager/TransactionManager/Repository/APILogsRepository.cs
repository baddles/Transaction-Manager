using System;
using Microsoft.IdentityModel.Tokens;
using TransactionManager.Interfaces;
using TransactionManager.Models.DBModels;
using TransactionManager.Models.DBModels.Tables;

namespace TransactionManager.Repository
{
	public class APILogsRepository: IAPILogs
	{
		readonly DatabaseContext _dbContext;
		public APILogsRepository(DatabaseContext dbContext)
		{
			this._dbContext = dbContext;
		}
        public void writeLogToDb(APILogs logs)
		{
			this._dbContext.Add(logs);
			this._dbContext.SaveChanges();
		}
		public Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable<APILogs> getAllLogs()
		{
			return (Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable<APILogs>)this._dbContext.APILogs.Select(logs => logs).OrderByDescending(logs => logs.ExecutionTime);
        }
    }
}

