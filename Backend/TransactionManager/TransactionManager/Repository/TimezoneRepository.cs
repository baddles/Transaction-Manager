using System;
using TransactionManager.Interfaces;
using TransactionManager.Models.DBModels;
using TransactionManager.Models.DBModels.Tables;

namespace TransactionManager.Repository
{
	public class TimezoneRepository : ITimezone
	{
        readonly DatabaseContext _dbContext;
        public TimezoneRepository(DatabaseContext dbContext)
		{
			this._dbContext = dbContext;
		}
        public Microsoft.EntityFrameworkCore.DbSet<UserTimezone> getUserTimezone()
		{
			if (this._dbContext == null)
			{
				throw new Exception("Cannot connect to database: Null DB Context");
			}
			return _dbContext.timezone;
		}
    }
}

