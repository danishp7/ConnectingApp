using ConnectingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Data
{
    public class UserRepository : IUserRepository
    {
        // to implement methods we need the context so we inject it
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            // eager loading i.e include to get all the photos associated with user
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            
            // we don't need to check if the user is null or not cz 'firstordefault' method returns null
            // if no user exists
            return user;

        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            // if it equals to 0 it means nothing has been saved
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
