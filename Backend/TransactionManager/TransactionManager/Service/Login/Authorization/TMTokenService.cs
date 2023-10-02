using System;
using Org.BouncyCastle.Asn1.Tsp;
using TransactionManager.Models.APIModels.Login.Authorization.TokenResponseModel;
using TransactionManager.Models.DBModels;
using TransactionManager.Models.DBModels.Tables;
using TransactionManager.Repository;

namespace TransactionManager.Service.Login.Authorization
{
	public class TransactionManagerTokenService : JwtService
	{
        private readonly DatabaseContext _dbContext;
		public TransactionManagerTokenService(DatabaseContext dbContext, IConfiguration configuration) : base(configuration)
		{
            this._dbContext = dbContext;
		}
        public bool checkToken(string refresh_token)
        {
            if (this._dbContext == null)
            {
                throw new InvalidOperationException("Cannot connect to database: Null DBContext");
            }
            TokenRepository tokenRepo = new TokenRepository(this._dbContext);
            if (tokenRepo.getToken().Where(t => t.token == new Guid(refresh_token)).FirstOrDefault() == null)
            {
                return false;
            }
            return true;
        }
        public TMTokenResponseModel generateToken(string username, DateTime api_execution_time, Guid guid)
        {
            if (this._dbContext == null)
            {
                throw new InvalidOperationException("Cannot connect to database: Null DBContext");
            }
            if (guid == null)
            {
                guid = Guid.NewGuid();
            }
            (string refresh_token, DateTime expiration_time) = saveRefreshToken(username, guid, api_execution_time);
            string access_token = generateJwtToken(username, expiration_time, guid);
            return new TMTokenResponseModel(access_token, this._config["jwt:token_type"], guid.ToString(), expiration_time);
        }
        private (string, DateTime) saveRefreshToken(string username, Guid guid, DateTime currentUTC)
        {
            TimezoneService tzService = new TimezoneService(this._dbContext);
            string userTZ = tzService.getUserTimezone(username);
            DateTime timeout_at = determineExpirationTime(currentUTC, userTZ);
            Token t = new Token();
            t.username = username;
            t.token = guid;
            t.expiration_time = (timeout_at.Equals(TimeZoneInfo.ConvertTime(new DateTime(timeout_at.Year, timeout_at.Month, timeout_at.Day, 23, 59, 59), TimeZoneInfo.FindSystemTimeZoneById(userTZ)))) ? timeout_at.AddSeconds(1).AddMinutes(15) : timeout_at.AddMinutes(15);
            TokenRepository tokenRepo = new TokenRepository(_dbContext);
            tokenRepo.deleteToken(username);
            tokenRepo.setToken(t);
            return (guid.ToString(), timeout_at);
        }
        private DateTime determineExpirationTime(DateTime currentUTC, string userTimezone = "UTC")
        {
            TimeZoneInfo tzInfo = TimeZoneInfo.FindSystemTimeZoneById(userTimezone);
            DateTime targetDateTime = TimeZoneInfo.ConvertTimeFromUtc(currentUTC, tzInfo);
            /** Determine expiration time:
             * (Morning session)
            * 6AM: Token expires at 3pm
            * 11AM: Token expires at 8PM
            * (Afternoon session)
            * After 3PM: Token expires at 11:59:59pm
            * 9PM: Token expires at 9am the next day.
            */
            if (targetDateTime.TimeOfDay >= TimeSpan.FromHours(6) && targetDateTime.TimeOfDay <= new TimeSpan(10, 59, 59))
            {
                return TimeZoneInfo.ConvertTime(new DateTime(targetDateTime.Year, targetDateTime.Month, targetDateTime.Day, 15, 0, 0), tzInfo);
            }
            else if (targetDateTime.TimeOfDay >= TimeSpan.FromHours(11) && targetDateTime.TimeOfDay <= new TimeSpan(14,59,59))
            {
                return TimeZoneInfo.ConvertTime(new DateTime(targetDateTime.Year, targetDateTime.Month, targetDateTime.Day, 20, 0, 0), tzInfo);
            }
            else if (targetDateTime.TimeOfDay >= TimeSpan.FromHours(15) && targetDateTime.TimeOfDay <= new TimeSpan(20,59,59))
            {
                return TimeZoneInfo.ConvertTime(new DateTime(targetDateTime.Year, targetDateTime.Month, targetDateTime.Day, 23, 59, 59), tzInfo);
            }
            else
            {
                return TimeZoneInfo.ConvertTime(new DateTime(targetDateTime.Year, targetDateTime.Month, targetDateTime.Day + 1, 9, 0, 0), tzInfo);
            }
        }
    }
}

