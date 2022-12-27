using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class BusService
    {

        private readonly BusRepository _busRepository;
        private readonly PayeeRepository _payeeRepository;
        public BusService(BusRepository busRepository, PayeeRepository payeeRepository)
        {
            _busRepository = busRepository;
            _payeeRepository = payeeRepository;
        }

        // Get bus by bus number 
        public async Task<(BusBody, Exception)> GetBus(string busNumber, int vendorId)
        {
            try
            {
                BusBody busBody;
                Bus bus = await _busRepository.GetBus(busNumber, vendorId);

                if (bus == null)
                {
                    return (null, new Exception("Something went wrong"));
                }

                busBody = new BusBody(bus);

                return (busBody, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public async IAsyncEnumerable<BusBody> GetAllBuses(int vendorId)
        {
            foreach (Bus bus in await _busRepository.GetAllBuses(vendorId))
            {
                yield return new BusBody(bus);
            }
        }

        // Get bus suggestion
        public async IAsyncEnumerable<BusBody> GetBusesByTerm(string term, int vendorId)
        {
            Queue<Bus> buses = await _busRepository.GetBusesByTerm(term, vendorId);

            foreach (Bus bus in await _busRepository.GetBusesByTerm(term, vendorId))
            {
                yield return new BusBody(bus);
            }
        }

        // Create new bus
        public async Task<(bool, Exception)> CreateBus(BusBody body, int vendorId)
        {

            try
            {
                bool result = false;
                Bus bus = await _busRepository.GetBus(body.BusNumber, vendorId);
                Payee payee = await _payeeRepository.GetPayee(body.PayeeName, vendorId);

                if (bus != null)
                {
                    return (false, new Exception($"Bus number {body.BusNumber} is already exist."));
                }

                if (payee == null)
                {
                    return (false, new Exception($"Payee name {body.PayeeName} not found"));
                }

                bus = new Bus(body.BusNumber, payee.Id, vendorId);

                result = await _busRepository.CreateBus(bus);

                if (result == false)
                {
                    return (false, new Exception("Something went wrong"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // Update bus
        public async Task<(bool, Exception)> UpdateBus(BusBody body, int vendorId)
        {
            try
            {
                bool result = false;
                Bus bus = await _busRepository.GetBus(body.BusNumber, vendorId);
                Payee payee = await _payeeRepository.GetPayee(body.PayeeName, vendorId);

                if (bus == null)
                {
                    return (false, new Exception($"Bus {body.BusNumber} Not found"));
                }

                if (payee == null)
                {
                    return (false, new Exception($"Payee {body.PayeeName} Not found"));
                }

                bus.BusNumber = body.BusNumber;
                bus.PayeeId = payee.Id;

                result = await _busRepository.UpdateBus(bus);

                if (result == false)
                {
                    return (false, new Exception("Could not update bus"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // Delete bus
        public async Task<(bool, Exception)> DeleteBus(string busNumber, int vendorId)
        {
            try
            {
                Bus bus = await _busRepository.GetBus(busNumber, vendorId);

                if (bus == null)
                {
                    return (false, new Exception($"Bus {busNumber} not found."));
                }

                await _busRepository.DeleteBus(bus);

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }
    }
}