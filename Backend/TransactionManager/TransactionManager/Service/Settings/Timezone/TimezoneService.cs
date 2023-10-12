using System;
using TransactionManager.Models.DBModels;
using TransactionManager.Repository;

namespace TransactionManager.Service
{
	public class TimezoneService
	{
		DatabaseContext _dbContext { get; set; }
		public TimezoneService(DatabaseContext dbContext)
		{
			if (dbContext == null)
			{
				throw new InvalidDataException("Cannot connect to DB: Null dbContext");
			}
			_dbContext = dbContext;
		}
		public async Task<string> getUserTimezone(string username)
		{
			TimezoneRepository tzRepo = new TimezoneRepository(_dbContext);
			string? userTimezone = await Task.Run(() => tzRepo.getUserTimezone().Where(user => user.username == username)
					.Select(timezone => timezone.timezone)
					.FirstOrDefault());
			if (userTimezone == null)
			{
				throw new InvalidDataException("Cannot find user's timezone. Please recheck DB");
			}
			return userTimezone;
		}
	}
}

