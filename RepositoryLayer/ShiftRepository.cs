using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class ShiftRepository
    {

        private readonly BusRouteDbContext _context;
        public ShiftRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateShift(Shift shift)
        {
            try
            {
                await _context.Shifts.AddAsync(shift);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public async Task<Shift> GetShift(int id)
        {
            try
            {
                Shift shift = await _context.Shifts.FirstOrDefaultAsync(shift => shift.Id == id);

                if (shift == null)
                {
                    throw new Exception("Shift not found.");
                }

                return shift;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Shift> GetShift(string name)
        {
            const int COMPARE_MATCH = 0;
            try
            {
                Shift shift = await _context.Shifts.FirstOrDefaultAsync(shift => string.Compare(shift.Name, name) == COMPARE_MATCH);

                if (shift == null)
                {
                    throw new Exception("Shift not found.");
                }

                return shift;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<bool> UpdateShift(Shift shift)
        {
            try
            {
                _context.Shifts.Update(shift);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
        public async Task<bool> DeleteShift(Shift shift)
        {
            try
            {
                _context.Shifts.Remove(shift);
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