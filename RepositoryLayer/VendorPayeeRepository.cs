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
            try
            {

                VendorPayee vendorPayee = await _context.VendorPayees.FirstOrDefaultAsync(vendorPayee => vendorPayee.Id == id);

                if (vendorPayee == null)
                {
                    throw new Exception("Vendor payee not found");
                }

                return vendorPayee;
            }
            catch (Exception e)
            {
                throw e;
            }
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
    }
}