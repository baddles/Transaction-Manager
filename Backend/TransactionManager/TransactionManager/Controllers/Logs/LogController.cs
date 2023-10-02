using System;
using Microsoft.AspNetCore.Mvc;
using TransactionManager.Models.DBModels;
using TransactionManager.Service.APILoggingService;
namespace TransactionManager.Controllers.Logs
{
	public class LogController : ControllerBase
	{
        private readonly DatabaseContext _dbContext;
        public LogController(DatabaseContext dbContext)
		{
			this._dbContext = dbContext;
		}
        
		[HttpGet]
		[Route("getLogs")]
		public IActionResult getLogFromDB()
		{
			return Ok(APILogsService.readFromDb(this._dbContext));
		}

    }
}

