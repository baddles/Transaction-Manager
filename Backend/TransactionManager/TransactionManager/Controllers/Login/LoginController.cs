using System;
using TransactionManager.Models.APIModels.Login;
using TransactionManager.Facade.Login;
using TransactionManager.Response;
using Microsoft.AspNetCore.Mvc;
using TransactionManager.Models.DBModels;
namespace TransactionManager.Controllers.Login
{
	[Route("auth")]
    [ApiController]
	public class LoginController: ControllerBase
	{
        private readonly ILogger<LoginController> _logger;
		private readonly DatabaseContext _dbContext;
		private IConfiguration _config;
        public LoginController(ILogger<LoginController> logger, DatabaseContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
			_config = new ConfigurationBuilder().AddJsonFile("usersettings.json")
			.Build();
        }
		[HttpGet]
		[Route("initialize")]
		public IActionResult processNewInstance([FromHeader] string state)
		{
			try
			{
                InitializeResponseModel responseModel = AuthenticationFacade.Initialize(state, _config);
                GenericResponseDTO<string, InitializeResponseModel> responseData = new GenericResponseDTO<string, InitializeResponseModel>(this._dbContext, HttpContext.Request.Method + " " + HttpContext.Request.Path, state, state,StatusCodes.Status200OK, "Success", DateTime.UtcNow, responseModel);
                return Ok(responseData);
            }
			catch (Exception ex)
			{
				GenericResponseDTO<string, string> error = new GenericResponseDTO<string, string>(this._dbContext,HttpContext.Request.Method + " " + HttpContext.Request.Path, state, state, StatusCodes.Status500InternalServerError, ex.Message, DateTime.UtcNow, ex.StackTrace.ToString());
				return StatusCode(StatusCodes.Status500InternalServerError, error);
			};
        }

        [HttpPost]
		[Route("login")]
		public IActionResult login([FromBody] LoginRequestModel userInfo)
		{
			try
			{
                if (!AuthenticationFacade.CheckPassword(_dbContext, userInfo.salt, userInfo.username, userInfo.encryptedPassword))
                {
					var jsonString = new
					{
						query = "SELECT * from USER where username='" + userInfo.username + "';"
					};
                    GenericResponseDTO<LoginRequestModel, dynamic> responseData = new GenericResponseDTO<LoginRequestModel, dynamic>(this._dbContext, HttpContext.Request.Method + " " + HttpContext.Request.Path, userInfo.salt, userInfo, StatusCodes.Status401Unauthorized, "Username or password incorrect", DateTime.UtcNow, jsonString);
					return Unauthorized(responseData);
                }
                LoginResponseModel res = AuthorizationFacade.getAuthorizationToken(userInfo.username, userInfo.code, _dbContext, _config);
                GenericResponseDTO<LoginRequestModel, LoginResponseModel> response = new GenericResponseDTO<LoginRequestModel, LoginResponseModel>(this._dbContext, HttpContext.Request.Method + " " + HttpContext.Request.Path, userInfo.username, userInfo, StatusCodes.Status200OK, "Success", DateTime.UtcNow, res);
                return Ok(response);
            }
			catch (Exception ex)
			{
                GenericResponseDTO<LoginRequestModel, string> error = new GenericResponseDTO<LoginRequestModel, string>(this._dbContext, HttpContext.Request.Method + " " + HttpContext.Request.Path, userInfo.salt, userInfo, StatusCodes.Status500InternalServerError, ex.Message, DateTime.UtcNow, ex.StackTrace.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
		}

		[HttpPost]
		[Route("refresh")]
		public IActionResult refresh([FromBody] string refresh_token)
		{
			return Ok("NOT IMPLEMENTED");
		}
    }
}

