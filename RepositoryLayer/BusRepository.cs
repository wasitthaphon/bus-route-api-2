using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class BusRepository
    {
        private readonly BusRouteDbContext _context;

        public BusRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBus(Bus bus)
        {
            try
            {
                await _context.Buses.AddAsync(bus);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public async Task<Bus> GetBus(int id)
        {

            try
            {
                Bus bus = await _context.Buses.FirstOrDefaultAsync(bus => bus.Id == id);
                return bus;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Queue<Bus>> GetBusesByTerm(string term, int vendorId)
        {
            Queue<Bus> buses = new Queue<Bus>();

            foreach (Bus bus in await
                            _context.Buses.Include(buses => buses.Payee).
                            Where(
                                bus => bus.BusNumber.ToUpper().Contains(term.ToUpper()) &&
                                bus.VendorId == vendorId).ToListAsync())
            {
                buses.Enqueue(bus);
            }

            return buses;
        }

        public async Task<Queue<Bus>> GetAllBuses()
        {
            Queue<Bus> buses = new Queue<Bus>();

            foreach (Bus bus in await _context.Buses.ToListAsync())
            {
                buses.Enqueue(bus);
            }

            return buses;
        }

        public async Task<Queue<Bus>> GetAllBuses(int vendorId)
        {
            Queue<Bus> buses = new Queue<Bus>();

            foreach (Bus bus in await _context.Buses
                                    .Include(buses => buses.Payee)
                                    .Where(buses => buses.VendorId == vendorId)
                                    .ToListAsync())
            {
                buses.Enqueue(bus);
            }

            return buses;
        }

        public async Task<Bus> GetBus(string busNumber, int vendorId)
        {
            const int COMPARE_MATCH = 0;

            try
            {
                Bus bus = await _context.Buses
                .Include(bus => bus.Payee)
                .FirstOrDefaultAsync(bus =>
                                    string.Compare(bus.BusNumber.ToUpper(), busNumber.ToUpper()) == COMPARE_MATCH &&
                                    bus.VendorId == vendorId);

                return bus;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> UpdateBus(Bus newBus)
        {
            try
            {
                _context.Buses.Update(newBus);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<bool> DeleteBus(Bus bus)
        {
            try
            {
                _context.Buses.Remove(bus);
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