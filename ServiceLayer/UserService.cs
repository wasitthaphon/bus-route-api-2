using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
using BusRouteApi.Misc;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusRouteApi.ServiceLayer
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly AppSettings _appSettings;
        public UserService(UserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<(bool, Exception)> CreateUser(UserBody body)
        {
            try
            {
                bool result;
                User user = await _userRepository.GetUser(body.Username);

                if (user != null)
                {
                    return (false, new Exception("Username already exist"));
                }

                user = new User(body.Username, HashPassword(body.Password), body.Name, body.Role, body.VendorId);
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
                user.Role = body.Role;
                user.VendorId = body.VendorId;

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

        public async Task<(bool, Exception)> DeleteUser(int id)
        {
            try
            {
                bool result;

                User user = await _userRepository.GetUser(id);

                if (user == null)
                {
                    return (false, new Exception("User does not exist."));
                }

                result = await _userRepository.DeleteUser(user);

                if (result == false)
                {
                    return (false, new Exception("Could not delete user."));
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

        public async IAsyncEnumerable<UserBody> GetUsers(string term, UserBody requester)
        {
            foreach (User user in await _userRepository.GetUsers(term, requester.VendorId))
            {
                yield return new UserBody
                {
                    Id = user.Id,
                    Name = user.Name,
                    Role = user.Role.ToString(),
                    Username = user.Username,
                    VendorId = user.VendorId
                };
            }
        }

        public async IAsyncEnumerable<UserBody> GetUsers(UserBody requester)
        {

            switch (GetRole(requester.Role))
            {
                case Role.Admin:

                    foreach (User user in await _userRepository.GetUsers())
                    {
                        yield return new UserBody
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Role = user.Role.ToString(),
                            Username = user.Username,
                            VendorId = user.VendorId
                        };
                    }
                    break;
                case Role.Clerk:
                    foreach (User user in await _userRepository.GetUsers(requester.VendorId))
                    {
                        yield return new UserBody
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Role = user.Role.ToString(),
                            Username = user.Username,
                            VendorId = user.VendorId
                        };
                    }
                    break;
            }
        }

        public async Task<(UserBody, Exception)> GetUser(int userId)
        {
            User user = await _userRepository.GetUser(userId);
            UserBody userBody;

            if (user == null)
            {
                return (null, new Exception("User not found"));
            }

            userBody = new UserBody();
            userBody.Id = user.Id;
            userBody.Name = user.Name;
            userBody.Username = user.Username;
            userBody.Role = user.Role.ToString();
            userBody.VendorId = user.VendorId;

            return (userBody, null);
        }

        public async Task<(AuthenticateResponse, Exception)> Authenticate(AuthenticateRequest request)
        {
            User user = await _userRepository.GetUser(request.Username);
            bool verified;
            if (user == null)
            {
                return (null, new Exception("User not found"));
            }

            verified = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!verified)
            {
                return (null, new Exception("Username or password incorrect."));
            }

            var token = generateJwtToken(user);

            return (new AuthenticateResponse(user, token), null);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(14),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}