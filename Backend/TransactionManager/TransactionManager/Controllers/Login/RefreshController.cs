using System;
using Microsoft.AspNetCore.Mvc;
using TransactionManager.Models.APIModels.Login;
using TransactionManager.Models.DBModels;
using TransactionManager.Response;

namespace TransactionManager.Controllers.Login
{
    [Route("auth")]
    [ApiController]
    public class RefreshController : ControllerBase
	{
        private readonly ILogger<RefreshController> _logger;
        private readonly DatabaseContext _dbContext;
        private IConfiguration _config;
        public RefreshController(ILogger<RefreshController> logger, DatabaseContext dbContext)
		{
            _logger = logger;
            _dbContext = dbContext;
            _config = new ConfigurationBuilder().AddJsonFile("usersettings.json").Build();
        }
        [HttpPost]
        [Route("refresh_imgur_token")]
        public IActionResult refreshImgur([FromBody] RefreshTokenRequestModel tokenInfo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                GenericResponseDTO<string, string> error = new GenericResponseDTO<string, string>(this._dbContext, HttpContext.Request.Method + " " + HttpContext.Request.Path, tokenInfo.username, tokenInfo.refresh_token, StatusCodes.Status500InternalServerError, ex.Message, DateTime.UtcNow, ex.StackTrace.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            return Unauthorized("NOT IMPLEMENTED");
        }

        [HttpPost]
        [Route("refresh_TM_token")]
        public IActionResult refreshTM([FromBody] RefreshTokenRequestModel tokenInfo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                GenericResponseDTO<string, string> error = new GenericResponseDTO<string, string>(this._dbContext, HttpContext.Request.Method + " " + HttpContext.Request.Path, tokenInfo.username, tokenInfo.refresh_token, StatusCodes.Status500InternalServerError, ex.Message, DateTime.UtcNow, ex.StackTrace.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            return Unauthorized("NOT IMPLEMENETED");
        }
    }
}

