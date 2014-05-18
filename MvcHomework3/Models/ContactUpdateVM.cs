﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcHomework3.Models
{
    public class ContactUpdateVM : IValidatableObject
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string Title { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "欄位長度不得大於 250 個字元")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string Mobile { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string Phone { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Email) &&
                RepositoryHelper.GetContactRepository().All()
                .Where(c => c.CustomerId == CustomerId)
                .Where(c => c.Id != Id)
                .Any(e => e.Email == Email))
                yield return new ValidationResult("Email duplicated.");
        }
    }
}