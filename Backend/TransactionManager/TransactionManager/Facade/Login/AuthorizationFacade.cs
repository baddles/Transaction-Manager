using System;
using TransactionManager.Models.DBModels;
using TransactionManager.Service.Login.Authorization;
using TransactionManager.Models.APIModels.Login;
using Microsoft.EntityFrameworkCore;
using TransactionManager.Service;

namespace TransactionManager.Facade.Login
{
	public static class AuthorizationFacade
	{
		public static async Task<LoginResponseModel> getAuthorizationToken(string username, DateTime apiExecutionTime, DatabaseContext _dbContext, IConfiguration _config)
		{
            TransactionManagerTokenService tmService = new TransactionManagerTokenService(_dbContext, _config);
			TimezoneService tzService = new TimezoneService(_dbContext);
            TokenBase token = await tmService.generateToken(username, apiExecutionTime, Guid.NewGuid());
			string timezone = await tzService.getUserTimezone(username);
			return new LoginResponseModel(token.access_token, token.token_type, token.refresh_token, token.expires_at, timezone);
		}
	}
}

