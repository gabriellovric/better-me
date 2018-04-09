using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BetterMeApi.Models
{
    public class Assignment
    {
        public long id { get; set; }
        public User user { get; set; }
        public Goal goal { get; set; }
        public DateTime expiration { get; set; }
        //public List<Progress> Progresses { get; set; }
    }
}