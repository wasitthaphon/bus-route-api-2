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
                DateOnly dateOnly;
                (dateOnly, e) = DateTimeParser.ParserDateFromString(body.RouteDate);

                if (e != null)
                {
                    return (false, new Exception("Could not parse date"));
                }

                // check date dup by route id and date
                RouteBody routeBody;
                (routeBody, e) = await _routeService.GetRoute(body.RouteName);

                if (e != null)
                {
                    routeBody = new RouteBody()
                    {
                        Name = body.RouteName,
                        Distance = body.Distance,
                        RouteType = body.RouteType
                    };

                    (result, e) = await _routeService.CreateRoute(routeBody);

                    if (result == false)
                    {
                        return (false, e);
                    }

                    (routeBody, e) = await _routeService.GetRoute(body.RouteName);
                }

                RoutePrice routePrice = await _routePriceRepository.GetRoutePrice(routeBody.Id, dateOnly);

                if (routePrice != null)
                {
                    return (false, new Exception("This route price already exist"));
                }

                routePrice = new RoutePrice(dateOnly, body.Price, routeBody.Id);

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
                DateOnly dateOnly;
                RouteBody routeBody;
                RoutePrice routePrice = await _routePriceRepository.GetRoutePrice(body.Id);

                (dateOnly, e) = DateTimeParser.ParserDateFromString(body.RouteDate);

                // check already exist
                if (routePrice.RouteId != body.RouteId || routePrice.RouteDate != dateOnly)
                {
                    RoutePrice newRoutePrice = await _routePriceRepository.GetRoutePrice(body.RouteId, dateOnly);

                    if (newRoutePrice != null)
                    {
                        return (false, new Exception("Route price already exist"));
                    }
                }

                routePrice.Price = body.Price;
                routePrice.RouteId = body.RouteId;
                routePrice.RouteDate = dateOnly;

                routeBody = new RouteBody()
                {
                    Id = body.RouteId,
                    Distance = body.Distance,
                    Name = body.RouteName,
                    RouteType = body.RouteType
                };

                (result, e) = await _routeService.UpdateRoute(routeBody);

                if (result == false)
                {
                    return (false, new Exception("Could not update route"));
                }

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

                remainingRoutePrice = await _routePriceRepository.GetRoutePrices(routePrice.Route.Id);

                if (remainingRoutePrice.Count == 0)
                {
                    (result, e) = await _routeService.DeleteRoute(routePrice.Route.Id);

                    if (result == false)
                    {
                        return (false, new Exception("Could not delete route"));
                    }
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


                (dateString, e) = DateTimeParser.DateOnlyToString(routePrice.RouteDate);

                routePriceBody = new RoutePriceBody()
                {
                    Id = routePrice.Id,
                    Price = routePrice.Price,
                    RouteDate = dateString,
                    RouteName = routePrice.Route.Name,
                    RouteType = routePrice.Route.RouteType,
                    Distance = routePrice.Route.Distance,
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
                (dateString, e) = DateTimeParser.DateOnlyToString(routePrice.RouteDate);
                yield return new RoutePriceBody()
                {
                    Id = routePrice.Id,
                    Price = routePrice.Price,
                    RouteDate = dateString,
                    RouteId = routePrice.RouteId,
                    RouteName = routePrice.Route.Name
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
                (dateString, e) = DateTimeParser.DateOnlyToString(routePrice.RouteDate);
                yield return new RoutePriceBody()
                {
                    Id = routePrice.Id,
                    Price = routePrice.Price,
                    RouteDate = dateString,
                    Distance = routePrice.Route.Distance,
                    RouteId = routePrice.RouteId,
                    RouteName = routePrice.Route.Name
                };
            }
        }
    }
}