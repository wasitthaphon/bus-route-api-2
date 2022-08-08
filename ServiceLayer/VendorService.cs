using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class VendorService
    {
        private readonly VendorRepository _vendorRepository;
        public VendorService(VendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<(bool, Exception)> CreateVendor(VendorBody body)
        {
            try
            {

                bool result;

                Vendor vendor = await _vendorRepository.GetVendor(body.Name);

                if (vendor != null)
                {
                    return (false, new Exception("Vendor already exist."));
                }

                vendor = new Vendor(body.Name);

                result = await _vendorRepository.CreateVendor(vendor);

                if (result == false)
                {
                    return (false, new Exception("Could not create vendor"));
                }

                return (true, null);

            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public async Task<(VendorBody, Exception)> GetFirstVendor()
        {
            try
            {
                VendorBody vendorBody;
                Vendor vendor;

                vendor = await _vendorRepository.GetDefaultVendor();

                if (vendor == null)
                {
                    return (null, new Exception("Vendor does not exist"));
                }

                vendorBody = new VendorBody();
                vendorBody.Id = vendor.Id;
                vendorBody.Name = vendor.Name;

                return (vendorBody, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public async Task<(bool, Exception)> UpdateVendor(VendorBody body)
        {
            try
            {
                bool result;
                const int COMPARE_MATCH = 0;

                Vendor vendor = await _vendorRepository.GetVendor(body.Id);

                if (vendor == null)
                {
                    return (false, new Exception("Vendor not found"));
                }

                if (string.Compare(vendor.Name, body.Name) != COMPARE_MATCH)
                {
                    Vendor newVendorName = await _vendorRepository.GetVendor(body.Name);

                    if (newVendorName != null)
                    {
                        return (false, new Exception("Vendor name already exist"));
                    }
                }

                vendor.Name = body.Name;

                result = await _vendorRepository.UpdateVendor(vendor);

                if (result == false)
                {
                    return (false, new Exception("Could not update vendor"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public async Task<(bool, Exception)> DeleteVendor(int id)
        {
            try
            {
                bool result;
                Vendor vendor = await _vendorRepository.GetVendor(id);

                if (vendor == null)
                {
                    return (false, new Exception("Vendor not found"));
                }

                result = await _vendorRepository.DeleteVendor(vendor);

                if (result == false)
                {
                    return (false, new Exception("Could not delete vendor"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }
    }
}