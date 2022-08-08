using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class VendorPayeeService
    {
        private readonly VendorPayeeRepository _vendorPayeeRepository;
        public VendorPayeeService(VendorPayeeRepository vendorPayeeRepository)
        {
            _vendorPayeeRepository = vendorPayeeRepository;
        }

        // create vendor-payee
        public async Task<(bool, Exception)> CreateVendorPayee(VendorPayeeBody body)
        {
            try
            {
                bool result;

                VendorPayee vendorPayee = await _vendorPayeeRepository.GetVendorPayee(body.VendorId, body.PayeeId);

                if (vendorPayee != null)
                {
                    return (false, new Exception("Could not create transaction"));
                }

                vendorPayee = new VendorPayee(body.VendorId, body.PayeeId);
                result = await _vendorPayeeRepository.CreateVendorPayee(vendorPayee);

                if (result == false)
                {
                    return (false, new Exception("Could not create transaction"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // remove vendor-payee
        public async Task<(bool, Exception)> DeleteVendorPayee(int id)
        {
            try
            {
                bool result;

                VendorPayee vendorPayee = await _vendorPayeeRepository.GetVendorPayee(id);

                if (vendorPayee == null)
                {
                    return (false, new Exception("Could not delete transaction"));
                }

                result = await _vendorPayeeRepository.DeleteVendorPayee(vendorPayee);

                if (result == false)
                {
                    return (false, new Exception("Could not delete transaction"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // remove by vendor
        public async Task<(bool, Exception)> DeleteVendorPayees(int vendorId)
        {
            try
            {
                bool result;

                Queue<VendorPayee> vendorPayees = new Queue<VendorPayee>();
                vendorPayees = await _vendorPayeeRepository.GetVendorPayees(vendorId);

                if (vendorPayees.Count > 0)
                {
                    result = await _vendorPayeeRepository.DeleteVendorPayees(vendorPayees.ToList());

                    if (result == false)
                    {
                        return (false, new Exception("Could not delete transaction"));
                    }
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // get payee by vendor
        public async IAsyncEnumerable<VendorPayeeBody> GetVendorPayees(int vendorId)
        {
            foreach (VendorPayee vendorPayee in await _vendorPayeeRepository.GetVendorPayees(vendorId))
            {
                yield return new VendorPayeeBody()
                {
                    Id = vendorPayee.Id,
                    PayeeId = vendorPayee.PayeeId,
                    VendorId = vendorPayee.VendorId
                };
            }
        }

        // get vendor-payee
        public async Task<(VendorPayeeBody, Exception)> GetVendorPayee(int id)
        {
            try
            {
                VendorPayeeBody vendorPayeeBody = new VendorPayeeBody();
                VendorPayee vendorPayee = await _vendorPayeeRepository.GetVendorPayee(id);

                if (vendorPayee == null)
                {
                    return (null, new Exception("Transaction does not exist"));
                }

                vendorPayeeBody.Id = vendorPayee.Id;
                vendorPayeeBody.PayeeId = vendorPayee.PayeeId;
                vendorPayeeBody.VendorId = vendorPayee.VendorId;

                return (vendorPayeeBody, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }
    }
}