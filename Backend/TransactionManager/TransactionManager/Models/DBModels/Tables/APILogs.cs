using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionManager.Models.DBModels.Tables
{
    [Table("APILogs")]
    public class APILogs
	{
		[Key]
		[Required]
		[MaxLength(160)]
		public string API { get; set; }

		[Key]
		[Required]
		[MaxLength(64)]
		public string Requestee { get; set; }


        [MaxLength(655535)]
        public string Params { get; set; }

        [Required]
		public int Status { get; set; }

		[Required]
		[MaxLength(255)]
		public string Message { get; set; }

		[Key]
		[Required]
		public DateTime ExecutionTime { get; set; }

		[MaxLength(65535)]
		public string Data { get; set; }
	}
}

