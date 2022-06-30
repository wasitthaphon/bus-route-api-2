using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class PayeeRepository
    {
        private readonly BusRouteDbContext _context;


        public PayeeRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePayee(Payee payee)
        {
            try
            {
                await _context.Payees.AddAsync(payee);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
        public async Task<Payee> GetPayee(int id)
        {
            try
            {
                Payee payee = await _context.Payees.FirstOrDefaultAsync(payee => payee.Id == id);

                if (payee == null)
                {
                    throw new Exception("Payee not found");
                }

                return payee;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Payee> GetPayee(string payeeName)
        {
            const int COMPARE_MATCH = 0;
            try
            {
                Payee payee = await _context.Payees.FirstOrDefaultAsync(payee => string.Compare(payee.Name, payeeName) == COMPARE_MATCH);

                return payee;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<bool> UpdatePayee(Payee newPayee)
        {
            try
            {
                _context.Payees.Update(newPayee);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
        public async Task<bool> DeletePayee(Payee payee)
        {
            try
            {
                _context.Payees.Remove(payee);
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