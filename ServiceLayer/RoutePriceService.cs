using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class RoutePriceService
    {
        private readonly RoutePriceRepository _routePriceRepository;
        private readonly RouteService _routeService;

        const string DMY_FORMAT = "dd-MM-yyyy";

        public RoutePriceService(RoutePriceRepository routePriceRepository, RouteService routeService)
        {
            _routePriceRepository = routePriceRepository;
            _routeService = routeService;
        }

        // Create route price
        public async Task<(bool, Exception)> CreateRoutePrice(RoutePriceBody body)
        {
            try
            {
                bool result;
                Exception e;

                RouteBody routeBody;
                (routeBody, e) = await _routeService.GetRoute(body.RouteId);

                if (routeBody == null)
                {
                    return (false, new Exception("Route not found."));
                }

                RoutePrice routePrice = await _routePriceRepository.GetRoutePrice(routeBody.Id, body.OilPriceReference);

                if (routePrice != null)
                {
                    return (false, new Exception("This route price already exist"));
                }

                routePrice = new RoutePrice(body.OilPriceReference, body.Price, routeBody.Id);

                result = await _routePriceRepository.CreateRoutePrice(routePrice);

                if (result == false)
                {
                    return (false, new Exception("Could not create route price"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // Update route price
        public async Task<(bool, Exception)> UpdateRoutePrice(RoutePriceBody body)
        {
            try
            {
                bool result;
                Exception e;
                RouteBody routeBody;

                RoutePrice routePrice = await _routePriceRepository.GetRoutePrice(body.Id);

                routePrice.Price = body.Price;
                routePrice.RouteId = body.RouteId;
                routePrice.OilPriceReference = body.OilPriceReference;

                result = await _routePriceRepository.UpdateRoutePrice(routePrice);

                if (result == false)
                {
                    return (false, new Exception("Could not update route price"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // Delete route price
        public async Task<(bool, Exception)> DeleteRoutePrice(int id)
        {
            try
            {
                bool result;
                Queue<RoutePrice> remainingRoutePrice = new Queue<RoutePrice>();
                Exception e;

                RoutePrice routePrice = await _routePriceRepository.GetRoutePrice(id);

                if (routePrice == null)
                {
                    return (false, new Exception("Route price not found"));
                }

                result = await _routePriceRepository.DeleteRoutePrice(routePrice);

                if (result == false)
                {
                    return (false, new Exception("Could not delete route price"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // Get route price by id
        public async Task<(RoutePriceBody, Exception)> GetRoutePrice(int id)
        {
            try
            {
                string dateString;
                Exception e;
                RoutePrice routePrice = await _routePriceRepository.GetRoutePrice(id);
                RoutePriceBody routePriceBody;


                if (routePrice == null)
                {
                    return (null, new Exception("Route price not found"));
                }

                routePriceBody = new RoutePriceBody()
                {
                    Id = routePrice.Id,
                    Price = routePrice.Price,
                    OilPriceReference = routePrice.OilPriceReference,
                    RouteId = routePrice.RouteId
                };

                return (routePriceBody, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        // Get route price by route id
        public async IAsyncEnumerable<RoutePriceBody> GetRoutePrices(int id)
        {
            string dateString;
            Exception e;
            foreach (RoutePrice routePrice in await _routePriceRepository.GetRoutePrices(id))
            {
                yield return new RoutePriceBody()
                {
                    Id = routePrice.Id,
                    Price = routePrice.Price,
                    OilPriceReference = routePrice.OilPriceReference,
                    RouteId = routePrice.RouteId,
                };
            }
        }

        // Get route prices
        public async IAsyncEnumerable<RoutePriceBody> GetRoutePrices()
        {
            string dateString;
            Exception e;
            foreach (RoutePrice routePrice in await _routePriceRepository.GetRoutePrices())
            {
                yield return new RoutePriceBody()
                {
                    Id = routePrice.Id,
                    Price = routePrice.Price,
                    OilPriceReference = routePrice.OilPriceReference,
                    RouteId = routePrice.RouteId,
                };
            }
        }
    }
}