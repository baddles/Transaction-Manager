using System;
using TransactionManager.Models.DBModels.Tables;
namespace TransactionManager.Interfaces
{
	public interface IAPILogs
	{
		public void writeLogToDb(APILogs logs);
		public Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable<APILogs> getAllLogs();
	}
}

