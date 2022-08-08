using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
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
                DateOnly dateOnly;
                Exception e;

                (dateOnly, e) = DateTimeParser.ParserDateFromString(body.OilPriceDate);

                OilPrice oilPrice = new OilPrice(dateOnly, body.Price);

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
                DateOnly dateOnly;
                Exception e;

                (dateOnly, e) = DateTimeParser.ParserDateFromString(body.OilPriceDate);

                if (oilPrice == null)
                {
                    return (false, new Exception("Oil price not found"));
                }

                if (oilPrice.OilPriceDate != dateOnly)
                {
                    OilPrice oilPriceNewDate = await _oilPriceRepository.GetOilPrice(dateOnly);
                    if (oilPriceNewDate != null)
                    {
                        return (false, new Exception("The date of oil price already created."));
                    }
                }

                oilPrice.OilPriceDate = dateOnly;
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

        public async Task<(OilPriceBody, Exception)> GetOilPrice(string date)
        {
            try
            {
                Exception e;
                DateOnly dateOnly;
                string dateString;
                (dateOnly, e) = DateTimeParser.ParserDateFromString(date);
                (dateString, e) = DateTimeParser.DateOnlyToString(dateOnly);

                OilPrice oilPrice = await _oilPriceRepository.GetOilPrice(dateOnly);
                OilPriceBody oilPriceBody = new OilPriceBody();


                if (oilPrice == null)
                {
                    return (null, new Exception("Oil price not found"));
                }

                oilPriceBody.Id = oilPrice.Id;
                oilPriceBody.Price = oilPrice.Price;
                oilPriceBody.OilPriceDate = dateString;

                return (oilPriceBody, null);

            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public async IAsyncEnumerable<OilPriceBody> GetOilPrices()
        {
            Exception e;
            string dateOnly;

            foreach (OilPrice oilPrice in await _oilPriceRepository.GetOilPrices())
            {
                (dateOnly, e) = DateTimeParser.DateOnlyToString(oilPrice.OilPriceDate);
                yield return new OilPriceBody
                {
                    Id = oilPrice.Id,
                    OilPriceDate = dateOnly,
                    Price = oilPrice.Price
                };
            }
        }

    }
}