using System.Collections.Generic;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public class AllCalendarItemsQuery : IVectorQuery<CalendarItem>
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<CalendarItem> Execute()
        {
            return db.CalendarItems.All();
        }
    }

    public interface IVectorQuery<T>
    {
        IEnumerable<T> Execute();
    }
}