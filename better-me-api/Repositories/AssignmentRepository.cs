using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BetterMeApi.Models;

namespace BetterMeApi.Repositories
{
    public class AssignmentRepository : IRepository<Assignment>
    {
        private readonly BetterMeContext _context;
        
        public AssignmentRepository(BetterMeContext context)
        {
            _context = context;
        }

        public IEnumerable<Assignment> All
        {
            get { return _context.Assignments.Include(item => item.Goal).Include(item => item.User); }
        }

        public IEnumerable<Assignment> AllByUser(long userId)
        {
            return All.Where(item => item.UserId == userId);
        }

        public IEnumerable<Assignment> AllByGoal(long goalId)
        {
            return All.Where(item => item.GoalId == goalId);
        }

        public IEnumerable<Assignment> AllByUserEmail(string email)
        {
            return All.Where(item => item.User.Email == email);
        }

        public bool DoesItemExist(long id)
        {
            return All.Any(item => item.AssignmentId == id);
        }

        public Assignment Find(long id)
        {
            return All.FirstOrDefault(item => item.AssignmentId == id);
        }

        public void Insert(Assignment item)
        {
            _context.Assignments.Add(item);
            _context.SaveChanges();
        }

        public void Update(Assignment item)
        {
            _context.Assignments.Update(item);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            _context.Assignments.Remove(this.Find(id));
            _context.SaveChanges();
        }
    }
}
