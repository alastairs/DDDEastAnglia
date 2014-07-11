using System.Collections.Generic;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public interface IVectorQuery<T>
    {
        IEnumerable<T> Execute();
    }
}