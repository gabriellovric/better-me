using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BetterMeApi.Models
{
    public class Progress
    {
        public long id { get; set; }
        public Assignment assignment { get; set; }
        public DateTime date { get; set; }
    }
}