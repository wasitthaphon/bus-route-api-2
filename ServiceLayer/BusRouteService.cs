using BusRouteApi.CombindModel;
using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
using BusRouteApi.Misc;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class BusRouteService
    {

        private readonly RouteRepository _routeRepository;
        private readonly BusRepository _busRepository;
        private readonly ShiftRepository _shiftRepository;
        private readonly OilPriceRepository _oilPriceRepository;
        private readonly BusRouteRepository _busRouteRepository;
        private readonly VendorRepository _vendorRepository;
        private readonly RoutePriceRepository _routePriceRepository;
        private readonly RouteDistanceRepository _routeDistanceRepository;
        public BusRouteService(BusRouteRepository busRouteRepository, RouteRepository routeRepository,
        OilPriceRepository oilPriceRepository, RoutePriceRepository routePriceRepository,
        VendorRepository vendorRepository, RouteDistanceRepository routeDistanceRepository,
        BusRepository busRepository, ShiftRepository shiftRepository)
        {
            _busRouteRepository = busRouteRepository;
            _routeRepository = routeRepository;
            _busRepository = busRepository;
            _oilPriceRepository = oilPriceRepository;
            _shiftRepository = shiftRepository;
            _routePriceRepository = routePriceRepository;
            _vendorRepository = vendorRepository;
            _routeDistanceRepository = routeDistanceRepository;
        }

        public async Task<(bool, Exception)> CreateBusRouteCopyFromDate(BusRouteCopyFromDateRequest body, int vendorId)
        {
            DateOnly copyFromDate;
            DateOnly copyToDate;
            string copyToDateString;
            bool result;
            Exception e;
            BusRouteBody busRouteBody;

            try
            {

                (copyFromDate, e) = DateTimeParser.ParserDateFromString(body.FromDate);

                if (e != null)
                {
                    return (false, e);
                }

                (copyToDate, e) = DateTimeParser.ParserDateFromString(body.ToDate);
                if (e != null)
                {
                    return (false, e);
                }

                (busRouteBody, e) = await GetBusRoutes(copyFromDate, vendorId);
                if (e != null)
                {
                    return (false, e);
                }

                (copyToDateString, e) = DateTimeParser.DateOnlyToString(copyToDate);
                busRouteBody.BusRouteDate = copyToDateString;

                (result, e) = await CreateBusRoute(busRouteBody, vendorId);
                if (e != null)
                {
                    return (false, e);
                }

            }
            catch (Exception ex)
            {
                return (false, ex);
            }


            return (true, null);
        }

        // create bus route
        public async Task<(bool, Exception)> CreateBusRoute(BusRouteBody body, int vendorId)
        {
            // validate all input
            List<BusOnRouteShiftModel> busOnRouteShiftModels = new List<BusOnRouteShiftModel>();
            List<BusShiftModel> busShiftModels = new List<BusShiftModel>();
            List<Shift> shifts = new List<Shift>();
            List<Bus> buses = new List<Bus>();
            List<BusRoute> busRoutes = new List<BusRoute>();

            DatabaseLayer.Models.Route route;
            Shift shift;
            Bus bus;
            BusOnRouteShiftModel busOnRouteShiftModel;
            BusShiftModel busShiftModel;
            OilPrice oilPrice;
            BusRoute busRoute;
            RoutePrice mRoutePrice;
            RouteDistance mRouteDistance;
            double routePrice;
            int routeDistance;
            int oilPriceReference;
            DateOnly oilPriceDate;
            Exception e;

            (oilPriceDate, e) = DateTimeParser.ParserDateFromString(body.BusRouteDate);

            bool result;

            oilPrice = await _oilPriceRepository.GetOilPrice(oilPriceDate, vendorId);
            if (oilPrice == null)
            {
                oilPrice = new OilPrice(oilPriceDate, 0, vendorId);
                await _oilPriceRepository.CreateOilPrice(oilPrice);
            }
            oilPrice = await _oilPriceRepository.GetOilPrice(oilPriceDate, vendorId);

            oilPriceReference = (int)Math.Floor(oilPrice.Price);

            (result, e) = await DeleteBusRoute(oilPriceDate, vendorId);

            if (body.details.Length == 0)
            {
                return (true, null);
            }


            for (int i = 0; i < body.details.Length; i++)
            {
                busOnRouteShiftModel = new BusOnRouteShiftModel();
                busShiftModels = new List<BusShiftModel>();
                // check route
                route = await _routeRepository.GetRoute(body.details[i].Route);
                if (route == null)
                {
                    return (false, new Exception($"Route not found {body.details[i].Route}"));
                }

                for (int j = 0; j < body.details[i].BusOnShifts.Length; j++)
                {

                    if (!string.IsNullOrEmpty(body.details[i].BusOnShifts[j].Shift) &&
                        !string.IsNullOrEmpty(body.details[i].BusOnShifts[j].BusNumber))
                    {
                        shift = await _shiftRepository.GetShift(body.details[i].BusOnShifts[j].Shift);
                        bus = await _busRepository.GetBus(body.details[i].BusOnShifts[j].BusNumber, vendorId);

                        if (shift == null)
                        {
                            return (false, new Exception("Shift not found"));
                        }

                        if (bus != null)
                        {
                            busShiftModel = new BusShiftModel()
                            {
                                Bus = bus,
                                Shift = shift
                            };

                            busShiftModels.Add(busShiftModel);
                        }
                    }
                }

                busOnRouteShiftModel = new BusOnRouteShiftModel()
                {
                    Route = route,
                    busRoutes = busShiftModels
                };

                busOnRouteShiftModels.Add(busOnRouteShiftModel);
            }

            // create transaction
            if (busOnRouteShiftModels.Count != body.details.Length)
            {
                return (false, new Exception("Could not create transaction"));
            }

            for (int i = 0; i < busOnRouteShiftModels.Count; i++)
            {

                mRoutePrice = await _routePriceRepository.GetRoutePrice(busOnRouteShiftModels[i].Route.Id, oilPriceReference);
                mRouteDistance = await _routeDistanceRepository.GetRouteDistanceLatest(busOnRouteShiftModels[i].Route.Id, oilPriceDate);

                if (mRoutePrice == null)
                {
                    routePrice = 0;
                }
                else
                {
                    routePrice = mRoutePrice.Price;
                }

                if (mRouteDistance == null)
                {
                    routeDistance = 0;
                }
                else
                {
                    routeDistance = mRouteDistance.Distance;
                }

                for (int j = 0; j < busOnRouteShiftModels[i].busRoutes.Count; j++)
                {
                    busShiftModel = busOnRouteShiftModels[i].busRoutes[j];

                    busRoute = new BusRoute(oilPriceDate,
                                            busShiftModel.Bus.Id,
                                            busShiftModel.Shift.Id,
                                            busOnRouteShiftModels[i].Route.Id,
                                            oilPrice.Id,
                                            vendorId,
                                            routePrice,
                                            routeDistance);
                    busRoutes.Add(busRoute);
                }
            }

            if (busRoutes.Count > 0)
            {
                result = await _busRouteRepository.CreateBusRoutes(oilPriceDate, vendorId, busRoutes);

                if (result == false)
                {
                    return (false, new Exception("Could not create bus route"));
                }
            }
            else
            {
                return (false, new Exception("Could not create transaction"));
            }

            return (true, null);
        }

        // delete bus route
        public async Task<(bool, Exception)> DeleteBusRoute(DateOnly busRouteDate, int vendorId)
        {
            try
            {
                bool result = false;
                Queue<BusRoute> busRoutes = new Queue<BusRoute>();

                busRoutes = await _busRouteRepository.GetBusRoutes(busRouteDate, vendorId);

                if (busRoutes.Count > 0)
                {
                    result = await _busRouteRepository.DeleteBusRoutes(busRoutes.ToList());
                }

                if (result == false)
                {
                    return (false, new Exception("Could not delete bus routes"));
                }



                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // get bus route by date
        public async Task<(BusRouteBody, Exception)> GetBusRoutes(DateOnly busRouteDate, int vendorId)
        {
            try
            {
                BusRouteBody busRouteBody = new BusRouteBody();
                List<BusOnShift> busOnShifts = new List<BusOnShift>();
                Queue<BusRoute> busRoutes = new Queue<BusRoute>();
                BusOnShift busOnShift;
                Dictionary<string, Queue<BusRoute>> busRoutesDict = new Dictionary<string, Queue<BusRoute>>();
                DatabaseLayer.Models.Route route;
                List<BusRouteBody> busRouteBodies = new List<BusRouteBody>();
                Shift shift;
                Bus bus;
                Exception e;
                bool result;

                List<BusRouteHeaderBody> busRouteHeaderBodies = new List<BusRouteHeaderBody>();
                BusRouteHeaderBody busRouteHeaderBody;

                List<RouteBody> routeBodies = new List<RouteBody>();
                List<ShiftBody> shiftBodies = new List<ShiftBody>();

                RouteBody routeBody;
                ShiftBody shiftBody;

                Queue<DatabaseLayer.Models.Route> routes = new Queue<DatabaseLayer.Models.Route>();
                Queue<Shift> shifts = new Queue<Shift>();

                List<BusRouteDetailBody> busRouteDetailBodies = new List<BusRouteDetailBody>();
                BusRouteDetailBody busRouteDetailBody;

                OilPrice oilPrice = await _oilPriceRepository.GetOilPrice(busRouteDate, vendorId);

                if (oilPrice == null)
                {
                    oilPrice = new OilPrice(busRouteDate, 0, vendorId);
                    await _oilPriceRepository.CreateOilPrice(oilPrice);
                }

                // Create bus route header bodies
                routes = await _routeRepository.GetAllRoutes(vendorId, "Active");
                shifts = await _shiftRepository.GetAllShifts();

                foreach (DatabaseLayer.Models.Route route1 in routes)
                {
                    routeBody = new RouteBody
                    {
                        Id = route1.Id,
                        Name = route1.Name,
                        RouteType = route1.RouteType,
                        Status = route1.Status,
                        VendorId = route1.VendorId
                    };

                    routeBodies.Add(routeBody);
                }

                foreach (Shift shift1 in shifts)
                {
                    shiftBody = new ShiftBody
                    {
                        Id = shift1.Id,
                        Name = shift1.Name,
                        Sequence = shift1.Sequence
                    };

                    shiftBodies.Add(shiftBody);
                }

                busRouteHeaderBody = new BusRouteHeaderBody();
                busRouteHeaderBody.routes = routeBodies.ToArray();
                busRouteHeaderBody.shifts = shiftBodies.ToArray();
                busRouteBody.headers = busRouteHeaderBody;

                // Create bus route details.
                busRoutes = await _busRouteRepository.GetBusRoutes(busRouteDate, vendorId);

                foreach (BusRoute busRoute in busRoutes)
                {

                    route = busRoute.Route;

                    if (route == null)
                    {
                        return (null, new Exception("Route missing"));
                    }

                    if (!busRoutesDict.ContainsKey(route.Name))
                    {
                        busRoutes = new Queue<BusRoute>();
                        busRoutes.Enqueue(busRoute);
                        busRoutesDict[route.Name] = busRoutes;
                    }
                    else
                    {
                        busRoutes = busRoutesDict[route.Name];
                        busRoutes.Enqueue(busRoute);
                        busRoutesDict[route.Name] = busRoutes;
                    }
                }

                if (busRoutes.Count > 0)
                {
                    busRouteBody.PageMode = "EDIT";
                }
                else
                {
                    busRouteBody.PageMode = "ENTRY";
                }

                foreach (RouteBody routeBody1 in routeBodies)
                {
                    busRouteDetailBody = new BusRouteDetailBody();
                    Queue<BusRoute> busRouteDict = new Queue<BusRoute>();

                    busRoutesDict.TryGetValue(routeBody1.Name, out busRouteDict);


                    if ((busRouteDict != null && routeBody1.RouteType == RouteType.Special.ToString()) ||
                                            (routeBody1.RouteType == RouteType.General.ToString()))
                    {
                        busRouteDetailBody.Route = routeBody1.Name;
                        busRouteDetailBody.IsSpecialRoute = routeBody1.RouteType == Misc.RouteType.Special.ToString() ? true : false;


                        busOnShifts = new List<BusOnShift>();
                        foreach (ShiftBody shiftBody1 in shiftBodies)
                        {
                            busOnShift = new BusOnShift();
                            busOnShift.Shift = shiftBody1.Name;
                            busOnShift.BusNumber = "";
                            if (busRouteDict != null)
                            {
                                foreach (BusRoute busRoute in busRouteDict)
                                {
                                    if (busRoute.Shift.Name == shiftBody1.Name)
                                    {
                                        busOnShift.BusNumber = busRoute.Bus.BusNumber;
                                        break;
                                    }
                                }
                            }

                            busOnShifts.Add(busOnShift);
                        }

                        busRouteDetailBody.BusOnShifts = busOnShifts.ToArray();
                        busRouteDetailBodies.Add(busRouteDetailBody);
                    }
                }

                busRouteBody.details = busRouteDetailBodies.ToArray();
                (busRouteBody.BusRouteDate, e) = DateTimeParser.DateOnlyToString(busRouteDate);
                busRouteBody.OilPrice = oilPrice.Price;


                return (busRouteBody, null);

            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public async Task<(string, Exception)> GetLatestExistDate(int vendorId)
        {
            try
            {

                string latestDate = string.Empty;
                BusRoute busRoute;
                Exception e;

                busRoute = await _busRouteRepository.GetLatestDate(vendorId);

                if (busRoute == null)
                {
                    return (latestDate, new Exception("No any data exists"));
                }

                (latestDate, e) = DateTimeParser.DateOnlyToString(busRoute.BusRouteDate);

                if (e != null)
                {
                    return (latestDate, e);
                }

                return (latestDate, null);
            }
            catch (Exception e)
            {
                return (string.Empty, e);
            }
        }

        public async Task<(BusRouteBodyByRoute[], Exception)> GetBusesOnRoute(BusRouteRequest request)
        {
            DatabaseLayer.Models.Route route;
            Queue<Shift> shifts = new Queue<Shift>();
            Queue<BusRoute> busRoutes = new Queue<BusRoute>();

            List<ShiftBody> shiftBodies = new List<ShiftBody>();
            ShiftBody shiftBody;

            List<BusRouteBodyByRoute> busRouteBodyByDates = new List<BusRouteBodyByRoute>();
            BusRouteBodyByRoute busRouteBodyByDate;

            List<BusOnShift> busOnShifts = new List<BusOnShift>();
            BusOnShift busOnShift;
            bool isFirst = true;

            DateOnly dateFrom;
            DateOnly dateTo;
            DateOnly tempBusRouteDate;
            string tempBusRouteDateString = string.Empty;
            Exception ex;

            (dateFrom, ex) = DateTimeParser.ParserDateFromString(request.DateFrom);

            if (ex != null)
            {
                throw ex;
            }

            (dateTo, ex) = DateTimeParser.ParserDateFromString(request.DateTo);
            if (ex != null)
            {
                throw ex;
            }

            // Create bus route header bodies
            route = await _routeRepository.GetRoute(request.RouteId);
            shifts = await _shiftRepository.GetAllShifts();


            busRoutes = await _busRouteRepository.GetBusRoutes(request.RouteId, dateFrom, dateTo);


            busRouteBodyByDate = new BusRouteBodyByRoute();
            foreach (BusRoute busRoute in busRoutes)
            {
                if (tempBusRouteDate != busRoute.BusRouteDate)
                {
                    busRouteBodyByDate = new BusRouteBodyByRoute();
                    (tempBusRouteDateString, ex) = DateTimeParser.DateOnlyToString(busRoute.BusRouteDate);
                    busOnShifts = new List<BusOnShift>();

                    foreach (Shift shift in shifts)
                    {
                        busOnShift = new BusOnShift();
                        busOnShift.Shift = shift.Name;
                        busOnShift.BusNumber = string.Empty;

                        if (shift.Name == busRoute.Route.Name)
                        {
                            busOnShift.BusNumber = busRoute.Bus.BusNumber;
                        }

                        busOnShifts.Add(busOnShift);
                    }

                    busRouteBodyByDate.BusRouteDate = tempBusRouteDateString;
                    busRouteBodyByDate.Route = busRoute.Route.Name;
                    busRouteBodyByDate.BusOnShifts = busOnShifts.ToArray();

                    tempBusRouteDate = busRoute.BusRouteDate;

                    if (!isFirst)
                    {
                        busRouteBodyByDates.Add(busRouteBodyByDate);
                    }

                    isFirst = false;
                }
                else
                {

                    for (int i = 0; i < busRouteBodyByDate.BusOnShifts.Length; i++)
                    {
                        if (busRouteBodyByDate.BusOnShifts[i].Shift == busRoute.Shift.Name)
                        {
                            busRouteBodyByDate.BusOnShifts[i].BusNumber = busRoute.Bus.BusNumber;
                        }
                    }
                }
            }

            return (busRouteBodyByDates.ToArray(), null);
        }
    }
}