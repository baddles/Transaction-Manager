using System;
using TransactionManager.Response;
using TransactionManager.Models.DBModels.Tables;
using TransactionManager.Models.DBModels;
using TransactionManager.Repository;
using Newtonsoft.Json;
namespace TransactionManager.Service.APILoggingService
{
	public static class APILogsService
	{
		public static APILogs[] readFromDb(DatabaseContext dbContext)
		{
			APILogsRepository repo = new APILogsRepository(dbContext);
			return repo.getAllLogs().ToArray<APILogs>();
        }
		public static void saveToDb(DatabaseContext dbContext, string apiName, string userOrState, string requestBody, int statusCodes, string message, DateTime timestamp, string data)
		{
            if (dbContext == null)
            {
                throw new InvalidOperationException("DBContext is not initialized.");
            }
            APILogs log = new APILogs();
			log.API = apiName;
			log.Requestee = userOrState;
			log.Params = requestBody;
			log.Status = statusCodes;
			log.Message = message;
			log.ExecutionTime = timestamp;
			string jsonData = JsonConvert.SerializeObject(data);
			log.Data = (jsonData.Length >= 65535) ? jsonData.Substring(0, 65535) : jsonData;
			APILogsRepository loggingRepo = new APILogsRepository(dbContext);
			loggingRepo.writeLogToDb(log);
		}
	}
}

