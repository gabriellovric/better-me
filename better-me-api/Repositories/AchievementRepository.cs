using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BetterMeApi.Models;

namespace BetterMeApi.Repositories
{
    public class AchievementRepository : IRepository<Achievement>
    {
        private readonly BetterMeContext _context;
        
        public AchievementRepository(BetterMeContext context)
        {
            _context = context;
        }

        public IEnumerable<Achievement> All
        {
            get { return _context.Achievements.Include(item => item.Goal); }
        }

        public IEnumerable<Achievement> AllWithQuery(long? userId, long? goalId)
        {
            var achievements = this.All;

            if (goalId != null)
            {
                achievements = achievements.Where(item => item.GoalId == goalId.Value);
            }
            if (userId != null)
            {
                //achievements = achievements.Where(item => item.GoalId == goalId.Value);
            }

            return achievements;
        }

        public bool DoesItemExist(long id)
        {
            return All.Any(item => item.AchievementId == id);
        }

        public Achievement Find(long id)
        {
            return All.FirstOrDefault(item => item.AchievementId == id);
        }

        public void Insert(Achievement item)
        {
            _context.Achievements.Add(item);
            _context.SaveChanges();
        }

        public void Update(Achievement item)
        {
            _context.Achievements.Update(item);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            _context.Achievements.Remove(this.Find(id));
            _context.SaveChanges();
        }
    }
}
