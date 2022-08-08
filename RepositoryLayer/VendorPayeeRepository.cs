using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class VendorPayeeRepository
    {
        private readonly BusRouteDbContext _context;
        public VendorPayeeRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateVendorPayee(VendorPayee vendorPayee)
        {
            try
            {
                await _context.VendorPayees.AddAsync(vendorPayee);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<VendorPayee> GetVendorPayee(int id)
        {

            VendorPayee vendorPayee = await _context.VendorPayees.FirstOrDefaultAsync(vendorPayee => vendorPayee.Id == id);

            return vendorPayee;
        }

        public async Task<VendorPayee> GetVendorPayee(int vendorId, int payeeId)
        {

            VendorPayee vendorPayee = await _context.VendorPayees.FirstOrDefaultAsync(vendorPayee => (vendorPayee.VendorId == vendorId) && (vendorPayee.PayeeId == payeeId));

            return vendorPayee;
        }

        public async Task<bool> UpdateVendorPayee(VendorPayee vendorPayee)
        {
            try
            {
                _context.VendorPayees.Update(vendorPayee);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<bool> DeleteVendorPayee(VendorPayee vendorPayee)
        {
            try
            {
                _context.VendorPayees.Remove(vendorPayee);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<Queue<VendorPayee>> GetVendorPayees(int vendorId)
        {
            Queue<VendorPayee> vendorPayees = new Queue<VendorPayee>();

            foreach (VendorPayee vendorPayee in await _context.VendorPayees.Where(vendorPayees => vendorPayees.VendorId == vendorId).ToListAsync())
            {
                vendorPayees.Enqueue(vendorPayee);
            }

            return vendorPayees;
        }

        public async Task<bool> DeleteVendorPayees(List<VendorPayee> vendorPayees)
        {
            try
            {

                _context.VendorPayees.RemoveRange(vendorPayees);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}