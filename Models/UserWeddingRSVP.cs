using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class UserWeddingRSVP
    {
        [Key]
        public int UserWeddingRSVPId { get; set; }
        public int UserId { get; set; }
        public int WeddingId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public User User { get; set; }
        public Wedding Wedding { get; set; }
    }
}