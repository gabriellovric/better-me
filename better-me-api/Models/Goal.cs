using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BetterMeApi.Models
{
    public enum Timeframe
    {
        Hour,
        Day,
        Week,
        Month,
        Year
    }

    public class Goal
    {
        [Required]
        public long GoalId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Timeframe Timeframe { get; set; }

        public string TimeframeText { get { return Timeframe.ToString(); } }

        public int Quantity { get; set; }

        
        public long UserId { get; set; }
        public User User { get; set; }

        public List<Achievement> Achievements { get; set; }

        public List<Assignment> Assignments { get; set; }
    }
}