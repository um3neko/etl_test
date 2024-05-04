#region Namespaces
using CsvHelper;
using ETL_test.DbContexts;
using ETL_test.Models.CVSModels;
using ETL_test.Models.EFModels;
using ETL_test.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
#endregion

namespace ETL_test.Repository.ObjectModelRep
{
    public class ObjectModelRepo : IObjectModelRepo
    {
        #region Fields
        private readonly ObjectModelDbContext _ctx;
        private readonly string _filePath;
        private readonly string _duplicateFilePath;
        #endregion

        #region Constructor
        public ObjectModelRepo(ObjectModelDbContext context)
        {
            _ctx = context;
            _filePath = Path.Combine(Environment.CurrentDirectory, "Public", "sample-cab-data.csv");
            _duplicateFilePath = Path.Combine(Environment.CurrentDirectory, "Public", "duplicates.csv");
        }
        #endregion

        /// <summary>
        /// Truncate table (purely for convenience)
        /// </summary>
        /// <returns></returns>
        /// 
        #region Public Methods
        public async Task<long> ClearData()
        {
            var stopwatch = Stopwatch.StartNew();
            await _ctx.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ObjectModels]");
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            return elapsedMilliseconds;
        }
        /// <summary>
        /// Retrieves the location ID with the highest average tip amount.
        /// </summary>
        /// <returns>An <see cref="OperationResult{T}"/> containing the location ID with the highest average tip amount and the execution time in milliseconds.</returns>
        public async Task<OperationResult<int>> GetHighestTipAmout()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var locationIdWithHighestAverageTipAmount = await _ctx.ObjectModels
                .GroupBy(x => x.PULocationID)
                .Select(g => new
                {
                    PULocationId = g.Key,
                    AverageTipAmount = g.Average(x => x.TipAmount)
                })
                .OrderByDescending(x => x.AverageTipAmount)
                .Select(x => x.PULocationId)
                .FirstOrDefaultAsync();
            stopwatch.Stop();

            return new OperationResult<int>
            {
                Value = locationIdWithHighestAverageTipAmount,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
        /// <summary>
        /// Retrieves the top 100 longest fares by trip distance.
        /// </summary>
        /// <returns>An <see cref="OperationResult{T}"/> containing the top 100 longest fares' pickup location IDs and the execution time in milliseconds.</returns>
        public async Task<OperationResult<IEnumerable<int>>> GetTopLongestFaresByTripDistance()
        {
            var stopwatch = Stopwatch.StartNew();

            var longest = await _ctx.ObjectModels
                .OrderByDescending(x => x.TripDistance)
                .Take(100)
                .Select(x => x.PULocationID)
                .ToListAsync();

            stopwatch.Stop();

            return new OperationResult<IEnumerable<int>>
            {
                Value = longest,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
        /// <summary>
        /// Retrieves the top 100 longest fares by time spent.
        /// </summary>
        /// <returns>An <see cref="OperationResult{T}"/> containing the top 100 longest fares' pickup location IDs and the execution time in milliseconds.</returns>
        public async Task<OperationResult<IEnumerable<int>>> GetTopLongestFaresByTimeSpend()
        {
            var stopwatch = Stopwatch.StartNew();

            var topLongestFares = await _ctx.ObjectModels
                .OrderByDescending(x => EF.Functions.DateDiffSecond(x.TPepPickupDateTime, x.TPepDropoffDateTime))
                .Select(x => x.Id)
                .Take(100)
                .ToListAsync();

            stopwatch.Stop();

            return new OperationResult<IEnumerable<int>>
            {
                Value = topLongestFares,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
        /// <summary>
        /// Loads data from a CSV file into the database.
        /// Data is read in chunks.
        /// </summary>
        /// <param name="convertToUtc">A flag indicating whether to convert the date and time values to UTC timezone.</param>
        /// <returns>An <see cref="OperationResult{T}"/>rows in table and the execution time in milliseconds.</returns>
        public async Task<OperationResult<int>> LoadData(bool convertToUtc = false)
        {
            var stopwatch = Stopwatch.StartNew();
            int batchSize = 2000;

            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CSVModelMapper>();
                var records = new List<ObjectModelCVS>();
                while (csv.Read())
                {
                    var record = csv.GetRecord<ObjectModelCVS>();

                    if (convertToUtc)
                    {
                        record.TPepPickupDateTime = TimeZoneHelper.ConvertESTtoUTS(record.TPepPickupDateTime);
                        record.TPepDropoffDateTime = TimeZoneHelper.ConvertESTtoUTS(record.TPepDropoffDateTime);
                    }

                    records.Add(record);
                    if (records.Count >= batchSize)
                    {
                        await Insert(records);
                        records.Clear();
                    }
                }
                if (records.Count != 0)
                {
                    await Insert(records);
                }
            }

            stopwatch.Stop();
            return new OperationResult<int>
            {
                Value = await _ctx.ObjectModels.CountAsync(),
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }


        /// <summary>
        /// Searches for ObjectModels by PULocationID.
        /// </summary>
        /// <param name="id">The PULocationID to search for.</param>
        /// <param name="predicate">An optional predicate to filter the results.</param>
        /// <returns>An OperationResult containing the IEnumerable of ObjectModels matching the search criteria and the execution time in milliseconds.</returns>
        public async Task<OperationResult<IEnumerable<ObjectModel>>> SearchByPUId(int id, Expression<Func<ObjectModel, bool>> predicate = null)
        {
            var stopwatch = Stopwatch.StartNew();

            IQueryable<ObjectModel> query = _ctx.Set<ObjectModel>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            query = query.Where(x => x.PULocationID == id);
            var result = await query.ToListAsync();
            stopwatch.Stop();

            return new OperationResult<IEnumerable<ObjectModel>>
            {
                Value = result,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
        /// <summary>
        /// Removes duplicate records based on certain criteria.
        /// </summary>
        /// <returns>An OperationResult containing the path of the file where duplicate records were saved and the execution time in milliseconds.</returns>
        public async Task<OperationResult<string>> RemoveDuplicate()
        {
            var stopwatch = Stopwatch.StartNew();
            var groupedDuplicates = await _ctx.ObjectModels
                .GroupBy(x => new { x.TPepPickupDateTime, x.TPepDropoffDateTime, x.PassengerCount })
                .ToListAsync();

            var duplicates = groupedDuplicates
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Skip(1))
                .AsEnumerable();

            using (var writer = new StreamWriter(_duplicateFilePath))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(duplicates);
            }

            _ctx.BulkDelete(duplicates);
            await _ctx.SaveChangesAsync();

            stopwatch.Stop();
            return new OperationResult<string>
            {
                Value = _filePath,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        /// Converts the store and forward flag values from 'N' to 'No' and 'Y' to 'Yes'.
        /// </summary>
        /// <returns>An OperationResult containing the number of records updated and the execution time in milliseconds.</returns>
        public async Task<OperationResult<int>> FwdFlagConverter()
        {
            var stopwatch = Stopwatch.StartNew();
            var recordsToUpdate = await _ctx.ObjectModels
                .Where(x => x.StoreAndFwdFlag == "N" || x.StoreAndFwdFlag == "Y")
                .ToListAsync();

            foreach (var record in recordsToUpdate)
            {
                record.StoreAndFwdFlag = record.StoreAndFwdFlag == "N" ? "No" : "Yes";
            }
            await _ctx.SaveChangesAsync();
            stopwatch.Stop();
            return new OperationResult<int>
            {
                Value = recordsToUpdate.Count, 
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
        #endregion
        /// <summary>
        /// Inserts a collection of ObjectModelCVS records into the database via fast bulk intering. Еhis gives a big speed boost.
        /// </summary>
        /// <param name="records">The collection of ObjectModelCVS records to insert.</param>
        #region Private Methods
        private async Task Insert(IEnumerable<ObjectModelCVS> records)
        {
            var objectModels = records.Select(record => new ObjectModel(record)).ToList();
            _ctx.BulkInsert(objectModels);
            await _ctx.SaveChangesAsync();
        }

        #endregion
    }
}
