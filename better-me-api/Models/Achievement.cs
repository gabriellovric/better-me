using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BetterMeApi.Models
{
    public class Achievement
    {
        public long id { get; set; }
        public Goal goal { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int achieved { get; set; }
    }
}