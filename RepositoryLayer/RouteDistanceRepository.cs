using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class RouteDistanceRepository
    {
        private readonly BusRouteDbContext _context;

        public RouteDistanceRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRouteDistance(RouteDistance routeDistance)
        {
            try
            {
                await _context.RouteDistances.AddAsync(routeDistance);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<RouteDistance> GetRouteDistance(int id)
        {
            RouteDistance routeDistance = await _context.RouteDistances.FirstOrDefaultAsync(routeDistance => routeDistance.Id == id);

            return routeDistance;
        }

        public async Task<RouteDistance> GetRouteDistanceLatest(int routeId, DateOnly latestDate)
        {
            RouteDistance routeDistance = await _context.RouteDistances.Include(routeDistance => routeDistance.Route)
                                                        .Where(routeDistance =>
                                                        routeDistance.Route.Id == routeId &&
                                                        latestDate >= routeDistance.RouteDate)
                                                        .OrderByDescending(routeDistance => routeDistance.RouteDate).FirstOrDefaultAsync();

            return routeDistance;
        }

        public async Task<RouteDistance> GetRouteDistanceOnDate(int routeId, DateOnly date)
        {
            RouteDistance routeDistance = await _context.RouteDistances.FirstOrDefaultAsync(routeDistance => routeDistance.RouteId == routeId && routeDistance.RouteDate == date);

            return routeDistance;
        }

        public async Task<RouteDistance> UpdateRouteDistance(RouteDistance routeDistance)
        {
            try
            {
                _context.RouteDistances.Update(routeDistance);
                await _context.SaveChangesAsync();

                return routeDistance;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteRouteDistance(RouteDistance routeDistance)
        {
            try
            {
                _context.RouteDistances.Remove(routeDistance);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("This route distance is already in bus route data, could not delete.");
            }

            return true;
        }

        public async Task<DatabaseLayer.Models.Route> GetRouteDistances(int id)
        {
            DatabaseLayer.Models.Route route = await _context.Routes
                                            .Include(route => route.RouteDistances)
                                            .FirstOrDefaultAsync(route => route.Id == id);

            return route;
        }

    }
}