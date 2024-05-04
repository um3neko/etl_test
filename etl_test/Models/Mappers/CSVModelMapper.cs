using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using ETL_test.Models.CVSModels;

public static class ConverterHelper
{
    public static T ConvertFromString<T>(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return default(T);
        }

        Type targetType = typeof(T);

        try
        {
            if (targetType == typeof(int))
            {
                return (T)(object)int.Parse(text);
            }
            else if (targetType == typeof(float))
            {
                return (T)(object)float.Parse(text);
            }
            else if (targetType == typeof(DateTime))
            {

                return (T)(object)DateTime.Parse(text);
            }
            else
            {

                return (T)Convert.ChangeType(text, targetType);
            }
        }
        catch (Exception)
        {

            return default(T);
        }
    }
}

public class CSVModelMapper : ClassMap<ObjectModelCVS>
{
    /// <summary>
    /// Mapping with validation parsing.
    /// </summary>
    public CSVModelMapper()
    {
        Map(m => m.TPepPickupDateTime).Name("tpep_pickup_datetime").TypeConverter<DateTimeConverter>();
        Map(m => m.TPepDropoffDateTime).Name("tpep_dropoff_datetime").TypeConverter<DateTimeConverter>();
        Map(m => m.PassengerCount).Name("passenger_count").TypeConverter<IntConverter>();
        Map(m => m.TripDistance).Name("trip_distance").TypeConverter<FloatConverter>();
        Map(m => m.StoreAndFwdFlag).Name("store_and_fwd_flag").TypeConverter<StringConverter>();
        Map(m => m.PULocationID).Name("PULocationID").TypeConverter<IntConverter>();
        Map(m => m.DOLocationID).Name("DOLocationID").TypeConverter<IntConverter>();
        Map(m => m.FareAmount).Name("fare_amount").TypeConverter<FloatConverter>();
        Map(m => m.TipAmount).Name("tip_amount").TypeConverter<FloatConverter>();
    }

    private class DateTimeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return ConverterHelper.ConvertFromString<DateTime>(text);
        }
    }

    private class IntConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return ConverterHelper.ConvertFromString<int>(text);
        }
    }

    private class FloatConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return ConverterHelper.ConvertFromString<float>(text);
        }
    }

    private class StringConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text;
        }
    }
}