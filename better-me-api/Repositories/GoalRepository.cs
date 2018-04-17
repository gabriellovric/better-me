using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BetterMeApi.Models;

namespace BetterMeApi.Repositories
{
    public class GoalRepository : IRepository<Goal>
    {
        private readonly BetterMeContext _context;

        public GoalRepository(BetterMeContext context)
        {
            _context = context;
        }

        public IEnumerable<Goal> All
        {
            get { return _context.Goals.Include(item => item.User); }
        }

        public IEnumerable<Goal> AllByUser(long userId)
        {
            return All.Where(item => item.UserId == userId);
        }

        public bool DoesItemExist(long id)
        {
            return All.Any(item => item.GoalId == id);
        }

        public Goal Find(long id)
        {
            return All.FirstOrDefault(item => item.GoalId == id);
        }

        public void Insert(Goal item)
        {
            _context.Goals.Add(item);
            _context.SaveChanges();
        }

        public void Update(Goal item)
        {
            _context.Goals.Update(item);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            _context.Goals.Remove(this.Find(id));
            _context.SaveChanges();
        }
    }
}
