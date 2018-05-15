using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

/**
 * Model Object für Progress
 */
namespace BetterMeApi.Models
{
    public class Progress
    {
        public long ProgressId { get; set; }

        public DateTime Date { get; set; }

        public long AssignmentId { get; set; }

        public Assignment Assignment { get; set; }
    }
}