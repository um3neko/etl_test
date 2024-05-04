using ETL_test.Repository.ObjectModelRep;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ETL_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ObjectModelController : ControllerBase
    {
        private readonly ILogger<ObjectModelController> _logger;
        private readonly IObjectModelRepo _repo;


       public ObjectModelController(ILogger<ObjectModelController> logger, IObjectModelRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }
        /// <summary>
        /// Reads data from csv file in public folder
        /// </summary>
        /// <param name="convertToUtc"> Specify whether to convert date times to UTC. </param>
        /// <returns>Rows in table and the execution time in milliseconds.</returns>

        [HttpGet("read-data")]
        [SwaggerOperation(Summary = "Reads data from csv file in public folder", Description = "Param:Specify whether to convert date times to UTC. Return: Rows in table and the execution time in milliseconds. ")]
        public async Task<IActionResult> ReadData(bool convertToUtc = false)
        {
            try
            {
                var resp = await _repo.LoadData(convertToUtc);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Truncate table (purely for convenience)
        /// </summary>
        /// <returns></returns>
        [HttpGet("clear-data")]
        [SwaggerOperation(Summary = "Truncate table (purely for convenience)", Description = "Clean db to try again.")]

        public async Task<IActionResult> ClearData()
        {
            try
            {
                var resp = await _repo.ClearData();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Find the top 100 longest fares in terms of `trip_distance`.
        /// </summary>
        /// <returns>id + query speed</returns>
        [HttpGet("top-fares-by-distance")]
        [SwaggerOperation(Summary = "Find the top 100 longest fares in terms of `trip_distance`.", Description = "Return id + query speed")]
        public async Task<IActionResult> GetTopByDistance()
        {
            try
            {
                var resp = await _repo.GetTopLongestFaresByTripDistance();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        /// <summary>
        /// Find the top 100 longest fares in terms of time spent traveling.
        /// </summary>
        /// <returns>id + query speed</returns>
        [HttpGet("top-fares-by-time-spent")]
        [SwaggerOperation(Summary = "Find the top 100 longest fares in terms of time spent traveling.", Description = "Return id + query speed")]
        public async Task<IActionResult> GetTopByTime()
        {
            try
            {
                var resp = await _repo.GetTopLongestFaresByTimeSpend();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Search, where part of the conditions is `PULocationId`.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpGet("filter-by-pulocation")]
        [SwaggerOperation(Summary = "Search, where part of the conditions is `PULocationId`..", Description = "Param:PULocationId, return object model instance ")]
        public async Task<IActionResult> FilterByPULocation(int locationId)
        {
            try
            {
                var result = await _repo.SearchByPUId(locationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Identify and remove any duplicate records from the dataset based on a combination of `pickup_datetime`, `dropoff_datetime`, and `passenger_count`.
        /// Write all removed duplicates into a `duplicates.csv` file in public folder.
        /// </summary>
        /// <returns>query speed</returns>
        [HttpGet("remove-dubplicates")]
        [SwaggerOperation(Summary = "Remove Duplicates", Description = "Identify and remove any duplicate records from the dataset based on a combination of `pickup_datetime`," +
            " `dropoff_datetime`, and `passenger_count` Write all removed duplicates into a `duplicates.csv` file in public folder. Returns query speed + Count of duplicates.")]
        public async Task<IActionResult> RemoveDuplicates()
        {
            try
            {
                var result = await _repo.RemoveDuplicate();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// For the `store_and_fwd_flag` column, convert any 'N' values to 'No' and any 'Y' values to 'Yes' in the db.
        /// </summary>
        /// <returns></returns>
        [HttpGet("fwd-flag-converter")]
        [SwaggerOperation(Summary = "FwdFlagConverter", Description = "For the `store_and_fwd_flag` column, convert any 'N' values to 'No' and any 'Y' values to 'Yes' in the db. Return query speed. Count of swapped fields.")]
        public async Task<IActionResult> FwdFlagConverter()
        {
            try
            {
                var result = await _repo.FwdFlagConverter();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
