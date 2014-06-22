using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData
{
    public class CalendarItemRepository : ICalendarItemRepository
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");
    }

    public class GetCalendarItemFromCalendarEntryTypeQuery
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public CalendarItem Execute(CalendarEntryType entryType)
        {
            return db.CalendarItems.FindByEntryTypeString(entryType.ToString());
        }
    }
}
