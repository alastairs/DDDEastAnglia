using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess
{
    public interface ICalendarItemRepository
    {
        CalendarItem GetFromType(CalendarEntryType voting);
    }
}