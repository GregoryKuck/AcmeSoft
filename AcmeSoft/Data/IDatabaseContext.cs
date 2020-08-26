using System.Data;

namespace AcmeSoft.Data
{
    public interface IDatabaseContext
    {
        IDbConnection Connection { get; }
    }
}