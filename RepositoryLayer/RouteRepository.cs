using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class RouteRepository
    {
        private readonly BusRouteDbContext _context;
        public RouteRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRoute(DatabaseLayer.Models.Route route)
        {
            try
            {
                await _context.Routes.AddAsync(route);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<DatabaseLayer.Models.Route> GetRoute(int id)
        {
            try
            {
                DatabaseLayer.Models.Route route = await _context.Routes.FirstOrDefaultAsync(route => route.Id == id);

                if (route == null)
                {
                    throw new Exception("Route not found");
                }

                return route;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DatabaseLayer.Models.Route> GetRoute(string name)
        {
            const int COMPARE_MATCH = 0;
            try
            {
                DatabaseLayer.Models.Route route = await _context.Routes.FirstOrDefaultAsync(route => string.Compare(route.Name, name) == COMPARE_MATCH);

                if (route == null)
                {
                    throw new Exception("Route not found");
                }

                return route;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> UpdateRoute(DatabaseLayer.Models.Route newRoute)
        {
            try
            {
                _context.Routes.Update(newRoute);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<bool> DeleteRoute(DatabaseLayer.Models.Route route)
        {
            try
            {
                _context.Routes.Remove(route);
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