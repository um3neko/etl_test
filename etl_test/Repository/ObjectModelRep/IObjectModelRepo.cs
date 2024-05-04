using ETL_test.Models.EFModels;
using System.Linq.Expressions;

namespace ETL_test.Repository.ObjectModelRep
{
    public interface IObjectModelRepo
    {

        Task<OperationResult<int>> LoadData(bool convertToUtc = false);
        Task<long> ClearData();

        Task<OperationResult<int>> GetHighestTipAmout();

        Task<OperationResult<IEnumerable<int>>> GetTopLongestFaresByTimeSpend();
        Task<OperationResult<IEnumerable<int>>> GetTopLongestFaresByTripDistance();
        Task<OperationResult<IEnumerable<ObjectModel>>> SearchByPUId(int id, Expression<Func<ObjectModel, bool>> predicate = null);
        Task<OperationResult<string>> RemoveDuplicate();
        Task<OperationResult<int>> FwdFlagConverter();

    }
}