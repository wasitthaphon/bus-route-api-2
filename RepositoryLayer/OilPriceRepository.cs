using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class OilPriceRepository
    {

        private readonly BusRouteDbContext _context;
        public OilPriceRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOilPrice(OilPrice oilPrice)
        {
            try
            {
                await _context.OilPrices.AddAsync(oilPrice);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public async Task<OilPrice> GetOilPrice(int id)
        {
            try
            {
                OilPrice oilPrice = await _context.OilPrices.FirstOrDefaultAsync(oilPrice => oilPrice.Id == id);

                if (oilPrice == null)
                {
                    throw new Exception("Oil price not found");
                }

                return oilPrice;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<OilPrice> GetOilPrice(DateOnly date, int vendorId)
        {
            try
            {
                OilPrice oilPrice = await _context.OilPrices.FirstOrDefaultAsync(oilPrice => oilPrice.OilPriceDate == date && oilPrice.VendorId == vendorId);
                return oilPrice;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Queue<OilPrice>> GetOilPrices(int vendorId)
        {
            Queue<OilPrice> oilPrices = new Queue<OilPrice>();

            foreach (OilPrice oilPrice in await _context.OilPrices.Where(oilPrices => oilPrices.VendorId == vendorId).ToListAsync())
            {
                oilPrices.Enqueue(oilPrice);
            }

            return oilPrices;

        }

        public async Task<bool> UpdateOilPrice(OilPrice newOilPrice)
        {
            try
            {
                _context.OilPrices.Update(newOilPrice);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public async Task<bool> DeleteOilPrice(OilPrice oilPrice)
        {
            try
            {
                _context.OilPrices.Remove(oilPrice);
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