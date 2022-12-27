using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class UserRepository
    {
        private readonly BusRouteDbContext _context;
        public UserRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<User> GetUser(int id)
        {
            try
            {
                User user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Queue<User>> GetUsers(string term, int vendorId)
        {
            Queue<User> users = new Queue<User>();


            foreach (User user in await _context.Users.Where(users =>
                                    users.Username.ToUpper().Contains(term.Trim().ToUpper())
                                    && users.VendorId == vendorId
                                    ).ToListAsync())
            {
                users.Enqueue(user);
            }

            return users;
        }

        public async Task<Queue<User>> GetUsers()
        {
            Queue<User> users = new Queue<User>();

            foreach (User user in await _context.Users.Where(users => true).ToListAsync())
            {
                users.Enqueue(user);
            }


            return users;
        }

        public async Task<Queue<User>> GetUsers(int vendorId)
        {
            Queue<User> users = new Queue<User>();

            foreach (User user in await _context.Users.Where(users => users.VendorId == vendorId).ToListAsync())
            {
                users.Enqueue(user);
            }

            return users;
        }


        public async Task<User> GetUser(string username)
        {
            const int COMPARE_MATCH = 0;
            try
            {
                User user = await _context.Users.FirstOrDefaultAsync(user => string.Compare(user.Username, username) == COMPARE_MATCH);
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> UpdateUser(User newUser)
        {
            try
            {
                _context.Users.Update(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<bool> DeleteUser(User user)
        {
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
    }
}