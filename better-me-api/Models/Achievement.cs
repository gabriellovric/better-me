using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BetterMeApi.Models
{
    public class Achievement
    {
        public long AchievementId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Achieved { get; set; }

        public long GoalId { get; set; }

        public Goal Goal { get; set; }
    }
}