using System;
using Microsoft.EntityFrameworkCore;
using TransactionManager.Repository;
using TransactionManager.Models.DBModels.Tables;
namespace TransactionManager.Models.DBModels
{
	public class DatabaseContext: DbContext
	{
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<APILogs> APILogs { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Token> token { get; set; }
        public DbSet<UserTimezone> timezone { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<APILogs>().HasKey(log => new { log.API, log.Requestee, log.ExecutionTime });
            modelBuilder.Entity<Token>().HasKey(token => new { token.username, token.token });
            base.OnModelCreating(modelBuilder);
        }
    }

    
}

