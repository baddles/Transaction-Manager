using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionManager.Models.DBModels.Tables
{
    [Table("User")]
    public class User
    {
        [Key]
        [MaxLength(64)]
        public string username { get; set; }

        [Required]
        [Column(TypeName = "BINARY(64)")]
        public byte[] hashedPassword { get; set; }

    }
}

