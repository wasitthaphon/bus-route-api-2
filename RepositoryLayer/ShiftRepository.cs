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
            Shift shift = await _context.Shifts.FirstOrDefaultAsync(shift => string.Compare(shift.Name, name) == COMPARE_MATCH);
            return shift;
        }
        public async Task<Queue<Shift>> GetShifts(int fromSequence, int toSequence)
        {
            Queue<Shift> shifts = new Queue<Shift>();

            foreach (Shift shift in await _context.Shifts.Where(shifts => (shifts.Sequence >= fromSequence) && (shifts.Sequence <= toSequence)).ToListAsync())
            {
                shifts.Enqueue(shift);
            }

            return shifts;
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

        public async Task<bool> UpdateShifts(List<Shift> shifts)
        {
            try
            {

                _context.Shifts.UpdateRange(shifts);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Queue<Shift>> GetShifts(int minSequence)
        {
            Queue<Shift> shifts = new Queue<Shift>();

            foreach (Shift shift in await _context.Shifts.Where(shifts => shifts.Sequence >= minSequence).ToListAsync())
            {
                shifts.Enqueue(shift);
            }

            return shifts;
        }

        public async Task<Queue<Shift>> GetShifts(string term)
        {
            const int COMPARE_MATCH = 0;
            Queue<Shift> shifts = new Queue<Shift>();

            if (term.Trim().Length > 0)
            {
                foreach (Shift shift in await _context.Shifts.Where(shifts => shifts.Name.ToUpper().Contains(term.ToUpper())).ToListAsync())
                {
                    shifts.Enqueue(shift);
                }
            }
            else
            {
                foreach (Shift shift in await _context.Shifts.ToListAsync())
                {
                    shifts.Enqueue(shift);
                }
            }

            return shifts;
        }

        public async Task<Queue<Shift>> GetAllShifts()
        {
            Queue<Shift> shifts = new Queue<Shift>();

            foreach (Shift shift in await _context.Shifts.Where(shifts => true).OrderBy(shifts => shifts.Sequence).ToListAsync())
            {
                shifts.Enqueue(shift);
            };

            return shifts;
        }

        public async Task<int> CountShifts()
        {
            int count = 0;

            count = await _context.Shifts.Where(shift => true).CountAsync();

            return count;
        }
    }
}