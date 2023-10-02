using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TransactionManager.Models.DBModels.Tables
{
    [Table("Token")]
    public class Token
	{
        [Key]
        [MaxLength(64)]
        public string username { get; set; }

        [Key]
        [MaxLength(36)]
        public Guid token { get; set; }

        [Required]
        public DateTime expiration_time { get; set; }

	}
}

