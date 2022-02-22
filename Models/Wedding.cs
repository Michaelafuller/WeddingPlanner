using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "must be at least {1} characters ")]
        public string WedderOne { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "must be at least {1} characters ")]
        public string WedderTwo { get; set; }

        [Required]
        [FutureDate]
        public DateTime Date { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "must be at least 5 characters")]
        public string WeddingAddress { get; set; }
        public int CreatedBy { get; set; }


        public List<UserWeddingRSVP> UserWeddingRSVP { get; set; }

    }
}