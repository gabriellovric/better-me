using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BetterMeApi.Models;

namespace BetterMeApi.Repositories
{
    public class GoalRepository
    {
        private readonly BetterMeContext _context;
        private readonly UserRepository _userRepository;

        public GoalRepository(BetterMeContext context, UserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public IEnumerable<Goal> All
        {
            get { return _context.Goals.Include(goal => goal.User); }
        }

        public IEnumerable<Goal> AllByUser(long userId)
        {
            return _context.Goals.Where(goal => goal.User.UserId == userId);
        }

        public bool DoesItemExist(long id)
        {
            return _context.Goals.Any(goal => goal.GoalId == id);
        }

        public Goal Find(long id)
        {
            return _context.Goals.FirstOrDefault(goal => goal.GoalId == id);
        }

        public void Insert(Goal item)
        {
            _context.Goals.Add(item);
            _context.SaveChanges();
        }

        public void Update(Goal item)
        {
            var goalItem = this.Find(item.GoalId);
            goalItem.Name = item.Name;
            goalItem.Description = item.Description;
            goalItem.Timeframe = item.Timeframe;
            goalItem.Quantity = item.Quantity;
            
            _context.Goals.Update(goalItem);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            _context.Goals.Remove(this.Find(id));
            _context.SaveChanges();
        }
    }
}
