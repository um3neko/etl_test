namespace ETL_test.Utils
{
    public class TimeZoneHelper
    {
        public static DateTime ConvertESTtoUTS(DateTime dateTime)
        {
            TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, estTimeZone);
        }
    }
}
