using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TransactionManager.Models.DBModels.Tables
{
	[Table("UserTimezone")]
	public class UserTimezone
	{
		[Key]
		[MaxLength(64)]
		public string username { get; set; }

		[MaxLength(128)]
		public string timezone { get; set; }

	
	}
}

