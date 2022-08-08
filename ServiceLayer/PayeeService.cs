using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class PayeeService
    {

        private readonly PayeeRepository _payeeRepository;

        public PayeeService(PayeeRepository payeeRepository)
        {
            _payeeRepository = payeeRepository;
        }

        public async Task<(bool, Exception)> CreatePayee(PayeeBody body)
        {
            try
            {
                bool result = false;
                Payee payee = await _payeeRepository.GetPayee(body.Name);

                if (payee != null)
                {
                    return (false, new Exception("Payee already created."));
                }

                payee = new Payee(body.Name);

                result = await _payeeRepository.CreatePayee(payee);

                if (result == false)
                {
                    return (false, new Exception("Something went wrong."));
                }

                return (true, null);

            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public async Task<(PayeeBody, Exception)> GetPayee(int id)
        {
            try
            {
                PayeeBody payeeBody;
                Payee payee = await _payeeRepository.GetPayee(id);

                if (payee == null)
                {
                    return (null, new Exception("Payee not found"));
                }

                payeeBody = new PayeeBody(payee);

                return (payeeBody, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public async Task<(PayeeBody, Exception)> GetPayee(string payeeName)
        {
            try
            {
                PayeeBody payeeBody;
                Payee payee = await _payeeRepository.GetPayee(payeeName);

                if (payee == null)
                {
                    return (null, new Exception("Payee not found"));
                }

                payeeBody = new PayeeBody(payee);

                return (payeeBody, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public async IAsyncEnumerable<PayeeBody> GetPayees(string term)
        {
            Queue<Payee> payees = await _payeeRepository.GetPayees(term);

            foreach (Payee payee in payees)
            {
                yield return new PayeeBody(payee);
            }
        }

        public async IAsyncEnumerable<PayeeBody> GetAllPayees()
        {
            foreach (Payee payee in await _payeeRepository.GetAllPayees())
            {
                yield return new PayeeBody(payee);
            }
        }

        public async Task<(bool, Exception)> UpdatePayee(PayeeBody body)
        {
            try
            {

                bool result;
                Payee payee = await _payeeRepository.GetPayee(body.Id);

                if (payee == null)
                {
                    return (false, new Exception("Payee not found"));
                }

                payee.Name = body.Name;

                result = await _payeeRepository.UpdatePayee(payee);

                if (result == false)
                {
                    return (false, new Exception("Could not update payee"));
                }

                return (true, null);

            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public async Task<(bool, Exception)> DeletePayee(int id)
        {
            try
            {
                bool result;
                Payee payee = await _payeeRepository.GetPayee(id);

                if (payee == null)
                {
                    return (false, new Exception("Payee not found"));
                }

                result = await _payeeRepository.DeletePayee(payee);

                if (result == false)
                {
                    return (false, new Exception("Could not delete payee"));
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