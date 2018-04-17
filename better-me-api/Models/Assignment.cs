using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BetterMeApi.Models
{
    public class Assignment
    {
        public long AssignmentId { get; set; }

        public DateTime Expiration { get; set; }
        
        public long UserId { get; set; }

        public User User { get; set; }

        public long GoalId { get; set; }

        public Goal Goal { get; set; }

        //public List<Progress> Progresses { get; set; }
    }
}