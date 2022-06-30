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
                RoutePrice routePrice = await _context.RoutePrices.FirstOrDefaultAsync(routePrice => routePrice.Id == id);

                if (routePrice == null)
                {
                    throw new Exception("Route price not found");
                }

                return routePrice;
            }
            catch (Exception e)
            {
                throw e;
            }
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