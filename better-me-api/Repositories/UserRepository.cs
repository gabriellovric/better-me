using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BetterMeApi.Models;

namespace BetterMeApi.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly BetterMeContext _context;

        public UserRepository(BetterMeContext context)
        {
            _context = context;
        }

        public IEnumerable<User> All
        {
            get { return _context.Users; }
        }

        /**
         * Überprüft, ob User mit der id existiert.
         * 
         * @return User
         */
        public bool DoesItemExist(long id)
        {
            return All.Any(item => item.UserId == id);
        }

        /**
         * Überprüft, ob User mit der email existiert.
         * 
         * @return User
         */
        public bool DoesItemExist(string email)
        {
            return All.Any(item => item.Email == email);
        }

        /**
         * Sucht User mit id
         * 
         * @return User
         */
        public User Find(long id)
        {
            return All.FirstOrDefault(item => item.UserId == id);
        }

        /**
         * Sucht alle Users mit entsprechender email
         * 
         * @return User 
         */
        public IEnumerable<User> Query(string email)
        {
            return All.Where(item => item.Email == email);
        }


        public User Find(string email)
        {
            return this.Query(email).FirstOrDefault();
        }

        /**
         * Speichert User
         */
        public void Insert(User item)
        {
            _context.Users.Add(item);
            _context.SaveChanges();
        }

        public void Update(User item)
        {
            _context.Users.Update(item);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            _context.Users.Remove(this.Find(id));
            _context.SaveChanges();
        }
    }
}
