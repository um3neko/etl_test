using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETL_test.Models.CVSModels
{
    public class ObjectModelCVS
    {
        [Column("tpep_pickup_datetime")]
        public DateTime TPepPickupDateTime { get; set; }

        [Column("tpep_dropoff_datetime")]
        public DateTime TPepDropoffDateTime { get; set; }

        [Column("passenger_count")]
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

        /////////////////////

        [Ignore]
        [Column("payment_type")]
        public int PaymentType { get; set; }
        [Ignore]
        public int RatecodeID { get; set; }

        [Ignore]
        [Column("extra")]
        public float Extra { get; set; }

        [Ignore]
        [Column("mta_tax")]
        public float MtaTax { get; set; }

        [Ignore]
        [Column("tolls_amount")]
        public float TollsAmount { get; set; }

        [Ignore]
        [Column("total_amount")]
        public float TotalAmount { get; set; }

        [Ignore]
        [Column("congestion_surcharge")]
        public float CongestionSurcharge { get; set; }

    }
}
