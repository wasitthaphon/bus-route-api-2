using BusRouteApi.Misc;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class RouteService
    {
        private readonly RouteRepository _routeRepository;
        public RouteService(RouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }


        // Create
        public async Task<(bool, Exception)> CreateRoute(RouteBody body)
        {
            try
            {
                bool result;

                DatabaseLayer.Models.Route route = await _routeRepository.GetRoute(body.Name);

                if (route != null)
                {
                    return (false, new Exception("Route already created"));
                }

                route = new DatabaseLayer.Models.Route(body.Name, body.RouteType, body.VendorId);

                result = await _routeRepository.CreateRoute(route);

                if (result == false)
                {
                    return (false, new Exception("Could not create route"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        private RouteType GetRouteType(string routeType)
        {
            switch (routeType)
            {
                case "General":
                    return RouteType.General;
                case "Special":
                    return RouteType.Special;
                default:
                    return RouteType.Special;
            }
        }

        // Update
        public async Task<(bool, Exception)> UpdateRoute(RouteBody body)
        {
            try
            {
                const int COMPARE_MATCH = 0;
                bool result;
                // check duplicate name
                DatabaseLayer.Models.Route route = await _routeRepository.GetRoute(body.Id);

                if (string.Compare(route.Name.Trim().ToUpper(), body.Name.Trim().ToUpper()) != COMPARE_MATCH)
                {
                    DatabaseLayer.Models.Route newRoute = await _routeRepository.GetRoute(body.Name);
                    if (newRoute != null)
                    {
                        return (false, new Exception("Route already exist"));
                    }
                }

                route.Name = body.Name;
                route.VendorId = body.VendorId;
                route.RouteType = body.RouteType;
                route.Status = body.Status;

                result = await _routeRepository.UpdateRoute(route);

                if (result == false)
                {
                    return (false, new Exception("Could not update route"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // Delete
        public async Task<(bool, Exception)> DeleteRoute(int id)
        {
            try
            {
                bool result;
                DatabaseLayer.Models.Route route = await _routeRepository.GetRoute(id);

                if (route == null)
                {
                    return (false, new Exception("Route not found"));
                }

                result = await _routeRepository.DeleteRoute(route);

                if (result == false)
                {
                    return (false, new Exception("Could not delete route"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // Get 1 by id
        public async Task<(RouteBody, Exception)> GetRoute(int id)
        {
            try
            {
                DatabaseLayer.Models.Route route = await _routeRepository.GetRoute(id);
                RouteBody body = new RouteBody();

                if (route == null)
                {
                    return (null, new Exception("Route not found"));
                }

                body.Id = route.Id;
                body.Name = route.Name;
                body.VendorId = route.VendorId;
                body.RouteType = route.RouteType.ToString();
                body.Status = route.Status;

                return (body, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        // Get 1 by name
        public async Task<(RouteBody, Exception)> GetRoute(string name)
        {
            try
            {
                DatabaseLayer.Models.Route route = await _routeRepository.GetRoute(name);
                RouteBody body = new RouteBody();

                if (route == null)
                {
                    return (null, new Exception("Route not found"));
                }

                body.Id = route.Id;
                body.Name = route.Name;
                body.VendorId = route.VendorId;
                body.RouteType = route.RouteType.ToString();
                body.Status = route.Status;

                return (body, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        // Get Suggestion
        public async IAsyncEnumerable<RouteBody> GetRoutes(string nameTerm)
        {
            foreach (DatabaseLayer.Models.Route route in await _routeRepository.GetRoutes(nameTerm))
            {
                yield return new RouteBody()
                {
                    Id = route.Id,
                    Name = route.Name,
                    VendorId = route.VendorId,
                    RouteType = route.RouteType,
                    Status = route.Status
                };
            }
        }

        public async IAsyncEnumerable<RouteBody> GetRoutes(int vendorId, string status)
        {
            foreach (DatabaseLayer.Models.Route route in await _routeRepository.GetRoutes(vendorId, status))
            {
                yield return new RouteBody()
                {
                    Id = route.Id,
                    Name = route.Name,
                    VendorId = route.VendorId,
                    RouteType = route.RouteType,
                    Status = route.Status
                };
            }
        }

        public async IAsyncEnumerable<RouteBody> GetRoutesByType(string nameTerm, string type)
        {
            foreach (DatabaseLayer.Models.Route route in await _routeRepository.GetRoutesByType(nameTerm, type))
            {
                yield return new RouteBody()
                {
                    Id = route.Id,
                    Name = route.Name,
                    VendorId = route.VendorId,
                    RouteType = route.RouteType.ToString(),
                    Status = route.Status
                };
            }
        }
    }
}