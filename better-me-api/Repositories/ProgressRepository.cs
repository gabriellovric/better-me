using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BetterMeApi.Models;

namespace BetterMeApi.Repositories
{
    public class ProgressRepository : IRepository<Progress>
    {
        private readonly BetterMeContext _context;
        
        public ProgressRepository(BetterMeContext context)
        {
            _context = context;
        }

        public IEnumerable<Progress> All
        {
            get
            {
                return _context
                    .Progresses
                    .Include(item => item.Assignment)
                    .Include(item => item.Assignment.User)
                    .Include(item => item.Assignment.Goal);
            }
        }

        public IEnumerable<Progress> AllWithQuery(long? userId, long? goalId, long? assignmentId)
        {
            var progresses = this.All;
            if (assignmentId != null)
            {
                progresses = progresses.Where(item => item.AssignmentId == assignmentId.Value);
            }
            if (goalId != null)
            {
                progresses = progresses.Where(item => item.Assignment.GoalId == goalId.Value);
            }
            if (userId != null)
            {
                progresses = progresses.Where(item => item.Assignment.UserId == userId.Value);
            }

            return progresses;
        }
        
        public bool DoesItemExist(long id)
        {
            return All.Any(item => item.ProgressId == id);
        }

        public Progress Find(long id)
        {
            return All.FirstOrDefault(item => item.ProgressId == id);
        }

        public void Insert(Progress item)
        {
            _context.Progresses.Add(item);
            _context.SaveChanges();
        }

        public void Update(Progress item)
        {
            _context.Progresses.Update(item);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            _context.Progresses.Remove(this.Find(id));
            _context.SaveChanges();
        }
    }
}
