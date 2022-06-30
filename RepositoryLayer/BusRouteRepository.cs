using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class BusRouteRepository
    {

        private readonly BusRouteDbContext _context;
        public BusRouteRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBusRoute(BusRoute busRoute)
        {
            try
            {
                await _context.BusRoutes.AddAsync(busRoute);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<BusRoute> GetBusRoute(int id)
        {
            try
            {
                BusRoute busRoute = await _context.BusRoutes.FirstOrDefaultAsync(busRoute => busRoute.Id == id);

                if (busRoute == null)
                {
                    throw new Exception("Bus route not found");
                }

                return busRoute;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> UpdateBusRoute(BusRoute busRoute)
        {
            try
            {
                _context.BusRoutes.Update(busRoute);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw e;
            }

            return true;
        }

        public async Task<bool> DeleteBusRoute(BusRoute busRoute)
        {
            try
            {
                _context.BusRoutes.Remove(busRoute);
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