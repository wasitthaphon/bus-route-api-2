using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class RoutePriceRepository
    {

        private readonly BusRouteDbContext _context;
        public RoutePriceRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRoutePrice(RoutePrice routePrice)
        {
            try
            {
                await _context.RoutePrices.AddAsync(routePrice);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public async Task<RoutePrice> GetRoutePrice(int id)
        {
            try
            {
                RoutePrice routePrice = await _context.RoutePrices
                                    .Include(routePrice => routePrice.Route)
                                    .FirstOrDefaultAsync(routePrice => routePrice.Id == id);

                return routePrice;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<RoutePrice> GetInBoundRoutePrice(int id, DateOnly startDate)
        {
            try
            {
                RoutePrice routePrice = await _context.RoutePrices.Include(routePrice => routePrice.Route)
                                    .OrderByDescending(routePrice => routePrice.RouteDate <= startDate)
                                    .FirstOrDefaultAsync(routePrice => routePrice.Route.Id == id);

                return routePrice;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<RoutePrice> GetRoutePrice(int routeId, DateOnly routeDate)
        {
            try
            {
                RoutePrice routePrice = await _context.RoutePrices
                                        .Include(routePrice => routePrice.Route)
                                        .FirstOrDefaultAsync(routePrice => (routePrice.Id == routeId) && (routePrice.RouteDate == routeDate));

                return routePrice;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Queue<RoutePrice>> GetRoutePrices(int id)
        {
            Queue<RoutePrice> routePrices = new Queue<RoutePrice>();

            foreach (RoutePrice routePrice in await _context.RoutePrices.Include(routePrices => routePrices.Route).Where(routePrices => routePrices.RouteId == id).ToListAsync())
            {
                routePrices.Enqueue(routePrice);
            }

            return routePrices;
        }

        public async Task<Queue<RoutePrice>> GetRoutePrices()
        {
            Queue<RoutePrice> routePrices = new Queue<RoutePrice>();
            foreach (RoutePrice routePrice in await _context.RoutePrices.Include(routePrices => routePrices.Route).Where(routePrices => true).ToListAsync())
            {
                routePrices.Enqueue(routePrice);
            }

            return routePrices;
        }

        public async Task<bool> UpdateRoutePrice(RoutePrice routePrice)
        {
            try
            {
                _context.RoutePrices.Update(routePrice);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw e;
            }

            return true;
        }

        public async Task<bool> DeleteRoutePrice(RoutePrice routePrice)
        {
            try
            {
                _context.RoutePrices.Remove(routePrice);
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