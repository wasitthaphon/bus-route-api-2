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

        public async Task<bool> CreateBusRoutes(DateOnly busRouteDate, List<BusRoute> busRoutes)
        {
            try
            {
                Queue<BusRoute> busRoutesOld = new Queue<BusRoute>();

                foreach (BusRoute busRoute in await _context.BusRoutes.Where(busRoutes => busRoutes.BusRouteDate == busRouteDate).ToListAsync())
                {
                    busRoutesOld.Enqueue(busRoute);
                }


                await _context.Database.BeginTransactionAsync();

                if (busRoutesOld.Count > 0)
                {
                    _context.BusRoutes.RemoveRange(busRoutesOld);
                    await _context.SaveChangesAsync();
                }

                await _context.BusRoutes.AddRangeAsync(busRoutes);
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
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

        public async Task<Queue<BusRoute>> GetBusRoutes(DateOnly busRouteDate)
        {
            Queue<BusRoute> busRoutes = new Queue<BusRoute>();

            foreach (BusRoute busRoute in await _context.BusRoutes.
                                    Include(busRoutes => busRoutes.RoutePrice.Route).
                                    Include(busRoutes => busRoutes.Bus).
                                    Include(busRoutes => busRoutes.Shift).
            Where(busRoutes => busRoutes.BusRouteDate == busRouteDate).ToListAsync())
            {
                busRoutes.Enqueue(busRoute);
            }

            return busRoutes;
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

        public async Task<bool> DeleteBusRoutes(List<BusRoute> busRoutes)
        {
            try
            {
                _context.BusRoutes.RemoveRange(busRoutes);
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