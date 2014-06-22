using System.Collections.Generic;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData
{
    public class CalendarItemRepository : ICalendarItemRepository
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<CalendarItem> GetAll()
        {
            return db.CalendarItems.All();
        }

        public CalendarItem GetFromType(CalendarEntryType voting)
        {
            return db.CalendarItems.FindByEntryTypeString(voting.ToString());
        }
    }

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
