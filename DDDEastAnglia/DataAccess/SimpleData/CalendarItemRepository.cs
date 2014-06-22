using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData
{
    public class CalendarItemRepository : ICalendarItemRepository
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public CalendarItem GetFromType(CalendarEntryType voting)
        {
            return db.CalendarItems.FindByEntryTypeString(voting.ToString());
        }
    }
}
