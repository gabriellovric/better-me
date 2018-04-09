using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BetterMeApi.Models;

namespace BetterMeApi.Repositories
{
    public class UserRepository
    {
        private readonly BetterMeContext _context;

        public UserRepository(BetterMeContext context)
        {
            _context = context;
        }

        public IEnumerable<User> All
        {
            get { return _context.Users.Include(user => user.Goals); }
        }

        public bool DoesItemExist(long id)
        {
            return _context.Users.Any(item => item.UserId == id);
        }

        public bool DoesItemExist(string email)
        {
            return _context.Users.Any(item => item.Email == email);
        }

        public User Find(long id)
        {
            return _context.Users.FirstOrDefault(item => item.UserId == id);
        }

        public void Insert(User item)
        {
            _context.Users.Add(item);
            _context.SaveChanges();
        }

        public void Update(User item)
        {
            var userItem = this.Find(item.UserId);
            userItem.Email = item.Email;
            userItem.Firstname = item.Firstname;
            userItem.Lastname = item.Lastname;
            
            _context.Users.Update(userItem);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            _context.Users.Remove(this.Find(id));
            _context.SaveChanges();
        }
    }
}
