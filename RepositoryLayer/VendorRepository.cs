using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class VendorRepository
    {
        private readonly BusRouteDbContext _context;
        public VendorRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateVendor(Vendor vendor)
        {
            try
            {

                await _context.Vendors.AddAsync(vendor);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<Vendor> GetDefaultVendor()
        {
            try
            {
                Vendor vendor = await _context.Vendors.FirstOrDefaultAsync(vendor => true);
                return vendor;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Vendor> GetVendor(int id)
        {
            try
            {
                Vendor vendor = await _context.Vendors.FirstOrDefaultAsync(vendor => vendor.Id == id);

                if (vendor == null)
                {
                    throw new Exception("Vendor not found");
                }

                return vendor;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Vendor> GetVendor(string vendorName)
        {
            const int COMPARE_MATCH = 0;
            try
            {
                Vendor vendor = await _context.Vendors.FirstOrDefaultAsync(vendor => string.Compare(vendor.Name, vendorName) == COMPARE_MATCH);

                if (vendor == null)
                {
                    return null;
                }

                return vendor;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Queue<Vendor>> GetVendors()
        {
            Queue<Vendor> vendors = new Queue<Vendor>();

            try
            {
                foreach (Vendor vendor in await _context.Vendors.ToListAsync())
                {
                    vendors.Enqueue(vendor);
                }

                return vendors;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> UpdateVendor(Vendor newVendor)
        {
            try
            {

                _context.Vendors.Update(newVendor);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public async Task<bool> DeleteVendor(Vendor vendor)
        {
            try
            {
                _context.Vendors.Remove(vendor);
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