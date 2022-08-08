using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.RepositoryLayer;
using BusRouteApi.RequestModels;

namespace BusRouteApi.ServiceLayer
{
    public class ShiftService
    {
        private readonly ShiftRepository _shiftRepository;
        public ShiftService(ShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        // create 
        public async Task<(bool, Exception)> CreateShift(ShiftBody body)
        {
            try
            {
                bool result;

                Shift shift = await _shiftRepository.GetShift(body.Name);
                int count = await _shiftRepository.CountShifts();

                if (shift != null)
                {
                    return (false, new Exception("Shift already exist"));
                }

                shift = new Shift(count + 1, body.Name);

                result = await _shiftRepository.CreateShift(shift);

                if (result == false)
                {
                    return (false, new Exception("Could not create shift"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // update
        public async Task<(bool, Exception)> UpdateShift(ShiftBody body)
        {
            try
            {
                bool result;
                int start, end;

                Shift shift;
                Shift compareShift;
                Queue<Shift> shiftWillBeNextUpdate = new Queue<Shift>();

                shift = await _shiftRepository.GetShift(body.Id);
                compareShift = await _shiftRepository.GetShift(body.Name);
                // check name and sequence
                if (body.Id != compareShift.Id)
                {
                    return (false, new Exception("Could not change name same to other shift."));
                }

                if (body.Sequence < shift.Sequence)
                {
                    start = body.Sequence;
                    end = shift.Sequence - 1;

                    shift = UpdateValues(shift, body);
                    shiftWillBeNextUpdate.Enqueue(shift);

                    foreach (Shift tempShift in await _shiftRepository.GetShifts(start, end))
                    {
                        tempShift.Sequence = tempShift.Sequence + 1;
                        shiftWillBeNextUpdate.Enqueue(tempShift);
                    }

                }
                else if (body.Sequence > shift.Sequence)
                {
                    start = shift.Sequence + 1;
                    end = body.Sequence;

                    foreach (Shift tempShift in await _shiftRepository.GetShifts(start, end))
                    {
                        tempShift.Sequence = tempShift.Sequence - 1;
                        shiftWillBeNextUpdate.Enqueue(tempShift);
                    }

                    shift = UpdateValues(shift, body);
                    shiftWillBeNextUpdate.Enqueue(shift);
                }
                else
                {
                    shift = UpdateValues(shift, body);
                    shiftWillBeNextUpdate.Enqueue(shift);
                }

                result = false;

                if (shiftWillBeNextUpdate.Count > 0)
                {
                    result = await _shiftRepository.UpdateShifts(shiftWillBeNextUpdate.ToList());
                }

                if (result == false)
                {
                    return (false, new Exception("Could not update shifts"));
                }

                return (true, null);

            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        private Shift UpdateValues(Shift shift, ShiftBody body)
        {
            shift.Sequence = body.Sequence;
            shift.Name = body.Name;

            return shift;
        }

        // delete
        public async Task<(bool, Exception)> DeleteShift(int id)
        {
            try
            {
                int minSequence;
                bool result;
                Queue<Shift> shifts = new Queue<Shift>();

                Shift shift = await _shiftRepository.GetShift(id);

                if (shift == null)
                {
                    return (false, new Exception("Shift not found"));
                }

                minSequence = shift.Sequence;
                foreach (Shift shift1 in await _shiftRepository.GetShifts(minSequence))
                {
                    shift1.Sequence = shift1.Sequence - 1;
                    shifts.Enqueue(shift1);
                }

                result = false;
                if (shifts.Count > 0)
                {
                    result = await _shiftRepository.UpdateShifts(shifts.ToList());
                }

                if (result == false)
                {
                    return (false, new Exception("Could not update shift"));
                }

                result = await _shiftRepository.DeleteShift(shift);

                if (result == false)
                {
                    return (false, new Exception("Could not delete shift"));
                }

                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        // get 1
        public async Task<(ShiftBody, Exception)> GetShift(string name)
        {
            try
            {
                ShiftBody shiftBody = new ShiftBody();
                Shift shift = await _shiftRepository.GetShift(name);

                if (shift == null)
                {
                    return (null, new Exception("Shift not found"));
                }

                shiftBody.Id = shift.Id;
                shiftBody.Name = shift.Name;
                shiftBody.Sequence = shift.Sequence;

                return (shiftBody, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public async Task<(ShiftBody, Exception)> GetShift(int id)
        {

            try
            {
                ShiftBody shiftBody = new ShiftBody();
                Shift shift = await _shiftRepository.GetShift(id);

                if (shift == null)
                {
                    return (null, new Exception("Shift not found"));
                }

                shiftBody.Id = shift.Id;
                shiftBody.Name = shift.Name;
                shiftBody.Sequence = shift.Sequence;

                return (shiftBody, null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        // get all
        public async IAsyncEnumerable<ShiftBody> GetShifts(string term)
        {
            foreach (Shift shift in await _shiftRepository.GetShifts(term))
            {
                yield return new ShiftBody
                {
                    Id = shift.Id,
                    Name = shift.Name,
                    Sequence = shift.Sequence
                };
            }
        }

        public async IAsyncEnumerable<ShiftBody> GetAllShifts()
        {
            foreach (Shift shift in await _shiftRepository.GetAllShifts())
            {
                yield return new ShiftBody
                {
                    Id = shift.Id,
                    Name = shift.Name,
                    Sequence = shift.Sequence
                };
            }
        }
    }
}