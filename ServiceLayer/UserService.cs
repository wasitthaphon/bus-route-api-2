using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Misc;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class UserService
    {

        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool, Exception)> CreateUser(UserBody body)
        {
            try
            {
                bool result;
                User user = await _userRepository.GetUser(body.Username);
                Role role = GetRole(body.Role);

                if (user != null)
                {
                    return (false, new Exception("Username already exist"));
                }

                user = new User(body.Username, HashPassword(body.Passsword), body.Name, role);
                result = await _userRepository.CreateUser(user);

                if (result == false)
                {
                    return (false, new Exception("Could not create user"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        private Role GetRole(string role)
        {
            switch (role)
            {
                case "Admin":
                    return Role.Admin;
                case "Clerk":
                    return Role.Clerk;
                case "Manager":
                    return Role.Manager;
                case "CarCenter":
                    return Role.CarCenter;
                case "Guest":
                    return Role.Guest;
                default:
                    return Role.Guest;
            }
        }

        public async Task<(bool, Exception)> UpdateUser(UserBody body)
        {
            try
            {
                bool result;
                User user = await _userRepository.GetUser(body.Id);

                user.Name = body.Name;
                //user.Role = body.Role;

                result = await _userRepository.UpdateUser(user);

                if (result == false)
                {
                    return (false, new Exception("Could not update user"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public async Task<(bool, Exception)> GetUser(UserBody body)
        {
            try
            {
                bool result;

                User user = await _userRepository.GetUser(body.Username);

                if (user == null)
                {
                    return (false, new Exception("User not found"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        private string HashPassword(string password)
        {
            return password;
        }
    }
}