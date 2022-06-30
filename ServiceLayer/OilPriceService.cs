using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class OilPriceService
    {

        private readonly OilPriceRepository _oilPriceRepository;

        public OilPriceService(OilPriceRepository oilPriceRepository)
        {
            _oilPriceRepository = oilPriceRepository;
        }

        public async Task<(bool, Exception)> CreateOilPrice(OilPriceBody body)
        {
            try
            {
                bool result;
                OilPrice oilPrice = new OilPrice(body.Date, body.Price);

                result = await _oilPriceRepository.CreateOilPrice(oilPrice);

                if (result == false)
                {
                    return (false, new Exception("Could not create oil price"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public async Task<(bool, Exception)> UpdateOilPrice(OilPriceBody body)
        {
            try
            {
                bool result;
                OilPrice oilPrice = await _oilPriceRepository.GetOilPrice(body.Id);
                if (oilPrice == null)
                {
                    return (false, new Exception("Oil price not found"));
                }

                if (oilPrice.OilPriceDate != body.Date)
                {
                    OilPrice oilPriceNewDate = await _oilPriceRepository.GetOilPrice(body.Date);
                    if (oilPriceNewDate != null)
                    {
                        return (false, new Exception("The date of oil price already created."));
                    }
                }

                oilPrice.OilPriceDate = body.Date;
                oilPrice.Price = body.Price;

                result = await _oilPriceRepository.UpdateOilPrice(oilPrice);

                if (result == false)
                {
                    return (false, new Exception("Could not update oil price"));
                }

                return (true, null);

            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public async Task<(OilPriceBody, Exception)> GetOilPrice(DateOnly date)
        {
            try
            {
                OilPrice oilPrice = await _oilPriceRepository.GetOilPrice(date);
                OilPriceBody oilPriceBody = new OilPriceBody();

                if (oilPrice == null)
                {
                    return (null, new Exception("Oil price not found"));
                }

                oilPriceBody.Id = oilPrice.Id;
                oilPriceBody.Price = oilPrice.Price;
                oilPriceBody.Date = oilPrice.OilPriceDate;

                return (oilPriceBody, null);

            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

    }
}