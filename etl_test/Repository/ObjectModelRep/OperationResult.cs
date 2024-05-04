namespace ETL_test.Repository.ObjectModelRep
{
    public class OperationResult<T> : IOperationResult
    {
        public T? Value { get; set; }
        public long ExecutionTimeMs { get; set; }
    }

    public interface IOperationResult
    {
        long ExecutionTimeMs { get; set; }
    }
}
