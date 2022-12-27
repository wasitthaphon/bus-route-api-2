using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class RouteRepository
    {
        private readonly BusRouteDbContext _context;
        public RouteRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRoute(DatabaseLayer.Models.Route route)
        {
            try
            {
                await _context.Routes.AddAsync(route);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<DatabaseLayer.Models.Route> GetRoute(int id)
        {
            try
            {
                DatabaseLayer.Models.Route route = await _context.Routes.FirstOrDefaultAsync(route => route.Id == id);

                return route;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DatabaseLayer.Models.Route> GetRoute(string name)
        {
            const int COMPARE_MATCH = 0;
            try
            {
                DatabaseLayer.Models.Route route = await _context.Routes.FirstOrDefaultAsync(route => string.Compare(route.Name, name) == COMPARE_MATCH);

                return route;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Queue<DatabaseLayer.Models.Route>> GetRoutes(int vendorId, string status)
        {
            Queue<DatabaseLayer.Models.Route> routes = new Queue<DatabaseLayer.Models.Route>();


            switch (status)
            {
                case "Active" or "InActive":
                    foreach (DatabaseLayer.Models.Route route in await
                            _context.Routes.Where(routes => routes.VendorId == vendorId && routes.Status == status).OrderBy(routes => routes.Name).ToListAsync())
                    {
                        routes.Enqueue(route);
                    }
                    break;
                case "All":
                    foreach (DatabaseLayer.Models.Route route in await
                            _context.Routes.Where(routes => routes.VendorId == vendorId).OrderBy(routes => routes.Name).ToListAsync())
                    {
                        routes.Enqueue(route);
                    }
                    break;
                default:
                    break;

            }

            return routes;
        }

        public async Task<Queue<DatabaseLayer.Models.Route>> GetRoutes(string nameTerm)
        {
            Queue<DatabaseLayer.Models.Route> routes = new Queue<DatabaseLayer.Models.Route>();

            foreach (DatabaseLayer.Models.Route route in await _context.Routes.Where(routes => routes.Name.ToUpper().Contains(nameTerm.ToUpper())).OrderBy(routes => routes.Name).ToListAsync())
            {
                routes.Enqueue(route);
            }

            return routes;
        }

        public async Task<Queue<DatabaseLayer.Models.Route>> GetRoutesByType(string nameTerm, string type)
        {
            Queue<DatabaseLayer.Models.Route> routes = new Queue<DatabaseLayer.Models.Route>();

            foreach (DatabaseLayer.Models.Route route in await _context.Routes.Where(routes => routes.Name.ToUpper().Contains(nameTerm.ToUpper()) && routes.RouteType == type).OrderBy(routes => routes.Name).ToListAsync())
            {
                routes.Enqueue(route);
            }

            return routes;
        }

        public async Task<Queue<DatabaseLayer.Models.Route>> GetAllGeneralRoute()
        {
            Queue<DatabaseLayer.Models.Route> routes = new Queue<DatabaseLayer.Models.Route>();

            foreach (DatabaseLayer.Models.Route route in await _context.Routes.Where(routes => routes.RouteType == "General").OrderBy(routes => routes.Name).ToListAsync())
            {
                routes.Enqueue(route);
            }

            return routes;
        }

        public async Task<Queue<DatabaseLayer.Models.Route>> GetAllRoutes(int vendorId, string status)
        {
            Queue<DatabaseLayer.Models.Route> routes = new Queue<DatabaseLayer.Models.Route>();

            switch (status)
            {
                case "Active" or "InActive":

                    foreach (DatabaseLayer.Models.Route route in await _context.Routes.Where(routes => routes.VendorId == vendorId && routes.Status == status).OrderBy(routes => routes.Name).ToListAsync())
                    {
                        routes.Enqueue(route);
                    }
                    break;
                case "All":

                    foreach (DatabaseLayer.Models.Route route in await _context.Routes.Where(routes => routes.VendorId == vendorId).OrderBy(routes => routes.Name).ToListAsync())
                    {
                        routes.Enqueue(route);
                    }
                    break;
                default:
                    break;
            }

            return routes;
        }

        public async Task<bool> UpdateRoute(DatabaseLayer.Models.Route newRoute)
        {
            try
            {
                _context.Routes.Update(newRoute);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<bool> DeleteRoute(DatabaseLayer.Models.Route route)
        {
            try
            {
                _context.Routes.Remove(route);
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