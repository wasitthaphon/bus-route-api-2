using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class RouteDistanceService
    {
        private readonly RouteDistanceRepository _routeDistanceRepository;
        private readonly RouteService _routeService;

        const string DMY_FORMAT = "dd-MM-yyyy";

        public RouteDistanceService(RouteDistanceRepository routeDistanceRepository, RouteService routeService)
        {
            _routeDistanceRepository = routeDistanceRepository;
            _routeService = routeService;
        }

        public async Task<(RouteDistanceBody, Exception)> GetRouteDistance(int id)
        {
            try
            {

                RouteDistance routeDistance;
                RouteDistanceBody routeDistanceBody;
                Exception e;
                string routeDate = string.Empty;

                routeDistance = await _routeDistanceRepository.GetRouteDistance(id);

                (routeDate, e) = DateTimeParser.DateOnlyToString(routeDistance.RouteDate);

                routeDistanceBody = new RouteDistanceBody()
                {
                    Id = routeDistance.Id,
                    Distance = routeDistance.Distance,
                    RouteDate = routeDate,
                    RouteId = routeDistance.Id
                };

                return (routeDistanceBody, null);

            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public async IAsyncEnumerable<RouteDistanceBody> GetRouteDistances(int routeId)
        {

            DatabaseLayer.Models.Route route = await _routeDistanceRepository.GetRouteDistances(routeId);

            foreach (RouteDistance routeDistance in route.RouteDistances)
            {
                string routeDate = string.Empty;
                Exception e;

                (routeDate, e) = DateTimeParser.DateOnlyToString(routeDistance.RouteDate);

                yield return new RouteDistanceBody
                {
                    Id = routeDistance.Id,
                    Distance = routeDistance.Distance,
                    RouteDate = routeDate,
                    RouteId = routeDistance.RouteId
                };
            }
        }

        public async Task<(bool, Exception)> CreateRouteDistance(RouteDistanceBody body)
        {

            try
            {

                bool result;
                Exception e;
                RouteBody routeBody;
                DateOnly routeDate;
                RouteDistance routeDistance;

                (routeBody, e) = await _routeService.GetRoute(body.RouteId);

                if (routeBody == null)
                {
                    return (false, new Exception("Route not found."));
                }

                (routeDate, e) = DateTimeParser.ParserDateFromString(body.RouteDate);

                if (e != null)
                {
                    return (false, e);
                }

                routeDistance = await _routeDistanceRepository.GetRouteDistanceOnDate(body.RouteId, routeDate);

                if (routeDistance != null)
                {
                    return (false, new Exception("Route distance of the date already created."));
                }

                routeDistance = new RouteDistance(routeDate, body.Distance, body.RouteId);

                result = await _routeDistanceRepository.CreateRouteDistance(routeDistance);

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }

        }

        public async Task<(RouteDistanceBody, Exception)> UpdateRouteDistance(RouteDistanceBody body)
        {

            try
            {

                RouteDistanceBody routeDistanceBody;
                DateOnly routeDate;
                string routeDateString = string.Empty;
                Exception e;

                RouteDistance routeDistance = await _routeDistanceRepository.GetRouteDistance(body.Id);

                (routeDate, e) = DateTimeParser.ParserDateFromString(body.RouteDate);

                routeDistance.Distance = body.Distance;
                routeDistance.RouteDate = routeDate;

                routeDistance = await _routeDistanceRepository.UpdateRouteDistance(routeDistance);

                (routeDateString, e) = DateTimeParser.DateOnlyToString(routeDistance.RouteDate);

                routeDistanceBody = new RouteDistanceBody()
                {
                    Id = routeDistance.Id,
                    Distance = routeDistance.Distance,
                    RouteId = routeDistance.RouteId,
                    RouteDate = routeDateString
                };

                return (routeDistanceBody, null);

            }
            catch (Exception e)
            {
                return (null, e);
            }

        }

        public async Task<(bool, Exception)> DeleteRouteDistance(int id)
        {
            try
            {

                bool result;
                RouteDistance routeDistance = await _routeDistanceRepository.GetRouteDistance(id);

                result = await _routeDistanceRepository.DeleteRouteDistance(routeDistance);

                return (result, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }


    }
}