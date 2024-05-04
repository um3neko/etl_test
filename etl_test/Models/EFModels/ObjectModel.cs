using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using ETL_test.Models.CVSModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETL_test.Models.EFModels
{
    public class ObjectModel
    {
        public ObjectModel(ObjectModelCVS modelCVS)
        {
            TPepPickupDateTime = modelCVS.TPepPickupDateTime;
            TPepDropoffDateTime = modelCVS.TPepDropoffDateTime;
            PassengerCount = modelCVS.PassengerCount;
            StoreAndFwdFlag = modelCVS.StoreAndFwdFlag;
            DOLocationID = modelCVS.DOLocationID;
            PULocationID = modelCVS.PULocationID;
            FareAmount = modelCVS.FareAmount;
            TripDistance = modelCVS.TripDistance;
            TipAmount = modelCVS.TipAmount;
        }

        public ObjectModel() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("tpep_pickup_datetime")]
        public DateTime TPepPickupDateTime { get; set; }

        [Column("tpep_dropoff_datetime")]
        public DateTime TPepDropoffDateTime { get; set; }

        [Column("passenger_count")]
        [TypeConverter(typeof(Int32Converter))]
        public int PassengerCount { get; set; }

        [Column("trip_distance")]
        public float TripDistance { get; set; }

        [Column("store_and_fwd_flag")]
        public string StoreAndFwdFlag { get; set; }

        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }

        [Column("fare_amount")]
        public float FareAmount { get; set; }

        [Column("tip_amount")]
        public float TipAmount { get; set; }
    }
}
