namespace BusRouteApi.Helpers
{
    public class DateTimeParser
    {
        const string DATE_FORMAT = "d-M-yyyy";

        public static (DateOnly, Exception) ParserDateFromString(string dateString)
        {
            try
            {
                DateOnly dateOnly;
                if (DateOnly.TryParseExact(dateString, DATE_FORMAT, out dateOnly))
                {
                    return (dateOnly, null);
                }
                else
                {
                    return (DateOnly.MinValue, new Exception("Could not parse date."));
                }
            }
            catch (Exception e)
            {

                return (DateOnly.MinValue, e);
            }
        }

        public static (string, Exception) DateOnlyToString(DateOnly date)
        {
            try
            {
                string dateString = date.ToString(DATE_FORMAT);
                return (dateString, null);
            }
            catch (Exception e)
            {
                return ("", e);
            }
        }
    }
}