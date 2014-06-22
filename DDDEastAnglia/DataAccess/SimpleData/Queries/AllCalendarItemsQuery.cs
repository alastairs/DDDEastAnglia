using System.Collections.Generic;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public class AllCalendarItemsQuery : IAllCalendarItemsQuery
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<CalendarItem> Execute()
        {
            return db.CalendarItems.All();
        }
    }

    public interface IAllCalendarItemsQuery
    {
        IEnumerable<CalendarItem> Execute();
    }
}