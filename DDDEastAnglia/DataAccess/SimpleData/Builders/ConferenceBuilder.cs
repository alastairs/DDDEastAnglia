using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess.SimpleData.Builders
{
    public class ConferenceBuilder : IBuild<Conference, Domain.Conference>
    {
        private readonly IBuild<CalendarItem, CalendarEntry> calendarEntryBuilder;
        private readonly IAllCalendarItemsQuery allCalendarItemsQuery;

        public ConferenceBuilder(ICalendarItemRepository calendarItemRepository, IBuild<CalendarItem, CalendarEntry> calendarEntryBuilder)
            : this(calendarItemRepository, calendarEntryBuilder, new AllCalendarItemsQuery()) { }

        public ConferenceBuilder(ICalendarItemRepository calendarItemRepository, IBuild<CalendarItem, CalendarEntry> calendarEntryBuilder, IAllCalendarItemsQuery allCalendarItemsQuery)
        {
            this.calendarEntryBuilder = calendarEntryBuilder;
            this.allCalendarItemsQuery = allCalendarItemsQuery;
        }

        public Domain.Conference Build(Conference item)
        {
            if (item == null)
            {
                return null;
            }
            
            var conference = new Domain.Conference(item.ConferenceId, item.Name, item.ShortName, item.NumberOfTimeSlots, item.NumberOfTracks);
            var calendarItems = allCalendarItemsQuery.Execute();

            foreach (var calendarItem in calendarItems)
            {
                var calendarEntry = calendarEntryBuilder.Build(calendarItem);
                conference.AddToCalendar(calendarEntry);
            }

            return conference;
        }
    }
}
