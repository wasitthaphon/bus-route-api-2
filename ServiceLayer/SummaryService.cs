using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class SummaryService
    {
        private readonly ShiftService _shiftService;
        private readonly BusRouteService _busRouteService;
        private readonly RouteService _routeService;
        private readonly BusService _busService;
        private readonly SummaryRepository _summaryRepository;

        public SummaryService(ShiftService shiftService, BusRouteService busRouteService, RouteService routeService, SummaryRepository summaryRepository, BusService busService)
        {
            _shiftService = shiftService;
            _busRouteService = busRouteService;
            _routeService = routeService;
            _summaryRepository = summaryRepository;
            _busService = busService;
        }

        public async Task<(SummaryByBusResponse, Exception)> GetSummaryBuBus(SummaryByBusRequest request)
        {
            SummaryByBusResponse summaryByBusResponse;
            List<SummaryOfDateRoute> summaryOfDateRoutes = new List<SummaryOfDateRoute>();
            SummaryOfDateRoute summaryOfDateRoute;
            List<ShiftBody> shiftBodies = new List<ShiftBody>();
            List<string> shiftNames = new List<string>();
            List<string> routeInShifts = new List<string>();
            List<BusRoute> tempBusRoutes = new List<BusRoute>();
            Dictionary<string, List<BusRoute>> busRouteDict = new Dictionary<string, List<BusRoute>>();
            DateOnly dateFrom;
            DateOnly dateTo;
            DateOnly tempDateCompared;
            BusBody busBody;
            Exception e;
            string dateString;
            double totalPrice = 0;

            try
            {
                // Parse date
                (dateFrom, e) = DateTimeParser.ParserDateFromString(request.DateFrom);

                if (e != null)
                {
                    return (null, e);
                }

                (dateTo, e) = DateTimeParser.ParserDateFromString(request.DateTo);
                if (e != null)
                {
                    return (null, e);
                }

                (busBody, e) = await _busService.GetBus(request.BusNumber, request.VendorId);
                if (e != null)
                {
                    return (null, e);
                }

                // Get bus information

                // Create shifts
                await foreach (ShiftBody shiftBody in _shiftService.GetAllShifts())
                {
                    shiftBodies.Add(shiftBody);
                }


                foreach (ShiftBody shiftBody in shiftBodies)
                {
                    shiftNames.Add(shiftBody.Name);
                }


                foreach (BusRoute busRoute in await _summaryRepository.GetSummaryByBus(busBody, dateFrom, dateTo))
                {
                    totalPrice += (busRoute.RoutePrice * busRoute.OilPrice.Price);

                    (dateString, e) = DateTimeParser.DateOnlyToString(busRoute.BusRouteDate);

                    if (!busRouteDict.ContainsKey(dateString))
                    {
                        tempBusRoutes = new List<BusRoute>();
                        tempBusRoutes.Add(busRoute);
                        busRouteDict[dateString] = tempBusRoutes;
                    }
                    else
                    {
                        tempBusRoutes = busRouteDict[dateString];
                        tempBusRoutes.Add(busRoute);
                        busRouteDict[dateString] = tempBusRoutes;
                    }

                }

                foreach (KeyValuePair<string, List<BusRoute>> busRouteGroups in busRouteDict)
                {
                    summaryOfDateRoute = new SummaryOfDateRoute();
                    summaryOfDateRoute.Date = busRouteGroups.Key;
                    routeInShifts = new List<string>();

                    for (int i = 0; i < shiftBodies.Count; i++)
                    {
                        routeInShifts.Add("");
                    }

                    foreach (BusRoute busRoute in busRouteGroups.Value)
                    {
                        for (int i = 0; i < shiftBodies.Count; i++)
                        {
                            if (shiftBodies[i].Id == busRoute.ShiftId)
                            {
                                routeInShifts[i] = busRoute.Route.Name;
                            }
                        }
                    }

                    summaryOfDateRoute.Routes = routeInShifts.ToArray();
                    summaryOfDateRoutes.Add(summaryOfDateRoute);
                }

                summaryByBusResponse = new SummaryByBusResponse();
                summaryByBusResponse.Shifts = shiftNames.ToArray();
                summaryByBusResponse.SummaryOfDateRoutes = summaryOfDateRoutes.ToArray();
                summaryByBusResponse.TotalPrice = totalPrice;

                if (busRouteDict.Keys.Count == 0)
                {
                    summaryByBusResponse.IsQueryFound = false;
                }
                else
                {
                    summaryByBusResponse.IsQueryFound = true;
                }

                return (summaryByBusResponse, null);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<(SummaryByRouteResponse[], Exception)> GetSummaryByRoute(SummaryByRouteRequest request)
        {

            try
            {
                Dictionary<string, SummaryByRouteResponse> summaryByRouteResponseDict = new Dictionary<string, SummaryByRouteResponse>();
                List<SummaryByRouteResponse> summaryByRouteResponses = new List<SummaryByRouteResponse>();
                SummaryByRouteResponse summaryByRouteResponse;
                DateOnly fromDate;
                DateOnly toDate;
                Exception e;

                (fromDate, e) = DateTimeParser.ParserDateFromString(request.FromDate);

                if (e != null)
                {
                    return (null, e);
                }

                (toDate, e) = DateTimeParser.ParserDateFromString(request.ToDate);

                if (e != null)
                {
                    return (null, e);
                }


                foreach (BusRoute busRoute in await _summaryRepository.GetSummaryByRoute(fromDate, toDate))
                {

                    if (summaryByRouteResponseDict.ContainsKey(busRoute.Route.Name))
                    {
                        summaryByRouteResponse = summaryByRouteResponseDict[busRoute.Route.Name];
                        summaryByRouteResponse.LapCount += 1;
                        summaryByRouteResponse.NMBPrice += (380 + (busRoute.OilPrice.Price / 2.85 * busRoute.RouteDistance) + (busRoute.RouteDistance * 3.25));
                        summaryByRouteResponse.TotalPrice += (busRoute.RoutePrice * busRoute.OilPrice.Price);
                    }
                    else
                    {
                        summaryByRouteResponse = new SummaryByRouteResponse();
                        summaryByRouteResponse.RouteName = busRoute.Route.Name;
                        summaryByRouteResponse.LapCount = 1;
                        summaryByRouteResponse.NMBPrice = (380 + (busRoute.OilPrice.Price / 2.85 * busRoute.RouteDistance) + (busRoute.RouteDistance * 3.25));
                        summaryByRouteResponse.TotalPrice = (busRoute.RoutePrice * busRoute.OilPrice.Price);
                    }

                    summaryByRouteResponseDict[busRoute.Route.Name] = summaryByRouteResponse;
                }

                foreach (KeyValuePair<string, SummaryByRouteResponse> summaryData in summaryByRouteResponseDict)
                {
                    summaryByRouteResponses.Add(summaryData.Value);
                }

                return (summaryByRouteResponses.ToArray(), null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public async Task<(SummaryByPayeeResponse[], Exception)> GetSummaryByPayee(SummaryByPayeeRequest request)
        {
            try
            {
                List<SummaryByPayeeResponse> summaryByPayeeResponses = new List<SummaryByPayeeResponse>();
                Dictionary<string, SummaryByPayeeResponse> summaryByPayeeResponsesDict = new Dictionary<string, SummaryByPayeeResponse>();
                SummaryByPayeeResponse summaryByPayeeResponse;
                DateOnly dateFrom;
                DateOnly dateTo;
                Exception e;

                (dateFrom, e) = DateTimeParser.ParserDateFromString(request.FromDate);

                if (e != null)
                {
                    return (null, e);
                }

                (dateTo, e) = DateTimeParser.ParserDateFromString(request.ToDate);

                if (e != null)
                {
                    return (null, e);
                }



                foreach (BusRoute busRoute in await _summaryRepository.GetSummaryByPayee(dateFrom, dateTo, request.VendorId))
                {
                    if (summaryByPayeeResponsesDict.ContainsKey(busRoute.Bus.Payee.Name))
                    {
                        summaryByPayeeResponse = summaryByPayeeResponsesDict[busRoute.Bus.Payee.Name];
                    }
                    else
                    {
                        summaryByPayeeResponse = new SummaryByPayeeResponse();
                        summaryByPayeeResponse.PayeeName = busRoute.Bus.Payee.Name;
                    }
                    summaryByPayeeResponse.NMBPrice += (380 + (busRoute.OilPrice.Price / 2.85 * busRoute.RouteDistance) + (busRoute.RouteDistance * 3.25));
                    summaryByPayeeResponse.PayeeTotalPrice += (busRoute.RoutePrice);
                    summaryByPayeeResponse.TotalReceived = summaryByPayeeResponse.NMBPrice - summaryByPayeeResponse.PayeeTotalPrice;
                    summaryByPayeeResponsesDict[busRoute.Bus.Payee.Name] = summaryByPayeeResponse;
                }


                foreach (KeyValuePair<string, SummaryByPayeeResponse> summaryData in summaryByPayeeResponsesDict)
                {
                    summaryByPayeeResponses.Add(summaryData.Value);
                }

                return (summaryByPayeeResponses.ToArray(), null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }
    }
}