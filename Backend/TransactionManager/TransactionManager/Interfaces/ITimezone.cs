using System;
using TransactionManager.Models.DBModels.Tables;
namespace TransactionManager.Interfaces
{
	public interface ITimezone
	{
        // For now will not implement changing timezone.
        public Microsoft.EntityFrameworkCore.DbSet<UserTimezone> getUserTimezone();
    }
}

