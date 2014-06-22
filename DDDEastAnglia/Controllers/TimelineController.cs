﻿using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class TimelineController : Controller
    {
        private const string DateOnlyPattern = "dddd d MMMM yyyy";
        private const string TimeOnlyPattern = "H:mm";
        private const string DateAndTimePattern = "dddd d MMMM yyyy, H:mm";

        private readonly GetCalendarItemFromCalendarEntryTypeQuery getCalendarItemFromCalendarEntryTypeQuery;
        private readonly IDateTimeFormatter dateTimeFormatter;
        private readonly IDateTimePassedEvaluator dateTimePassedEvaluator;

        public TimelineController(ICalendarItemRepository calendarItemRepository, IDateTimeFormatter dateTimeFormatter, IDateTimePassedEvaluator dateTimePassedEvaluator)
            : this(calendarItemRepository, dateTimeFormatter, dateTimePassedEvaluator, new GetCalendarItemFromCalendarEntryTypeQuery())
        {
        }

        public TimelineController(ICalendarItemRepository calendarItemRepository, IDateTimeFormatter dateTimeFormatter, IDateTimePassedEvaluator dateTimePassedEvaluator, GetCalendarItemFromCalendarEntryTypeQuery getCalendarItemFromCalendarEntryTypeQuery)
        {
            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }

            if (dateTimeFormatter == null)
            {
                throw new ArgumentNullException("dateTimeFormatter");
            }

            if (dateTimePassedEvaluator == null)
            {
                throw new ArgumentNullException("dateTimePassedEvaluator");
            }
            

            if (getCalendarItemFromCalendarEntryTypeQuery == null)
            {
                throw new ArgumentNullException("getCalendarItemFromCalendarEntryTypeQuery");
            }

            this.dateTimeFormatter = dateTimeFormatter;
            this.dateTimePassedEvaluator = dateTimePassedEvaluator;

            this.getCalendarItemFromCalendarEntryTypeQuery = getCalendarItemFromCalendarEntryTypeQuery;
        }

        public ActionResult ConferenceDate()
        {
            var conference = getCalendarItemFromCalendarEntryTypeQuery.Execute(CalendarEntryType.Conference);
            string conferenceDate = dateTimeFormatter.FormatDate(conference.StartDate);
            return new ContentResult {Content = conferenceDate};
        }

        public ActionResult ConferenceTime()
        {
            var conference = getCalendarItemFromCalendarEntryTypeQuery.Execute(CalendarEntryType.Conference);
            string startTime = dateTimeFormatter.FormatTime(conference.StartDate);
            string endTime = dateTimeFormatter.FormatTime(conference.EndDate.Value);
            string conferenceTimes = string.Format("{0} to {1}", startTime, endTime);
            return new ContentResult {Content = conferenceTimes};
        }

        public ActionResult Details()
        {
            var sessionSubmission = getCalendarItemFromCalendarEntryTypeQuery.Execute(CalendarEntryType.SessionSubmission);
            var voting = getCalendarItemFromCalendarEntryTypeQuery.Execute(CalendarEntryType.Voting);
            var agendaPublished = getCalendarItemFromCalendarEntryTypeQuery.Execute(CalendarEntryType.AgendaPublished);
            var registraion = getCalendarItemFromCalendarEntryTypeQuery.Execute(CalendarEntryType.Registration);

            var sessionSubmissionOpens = new TimelineItemModel
            {
                PeriodDate = dateTimeFormatter.FormatStartDate(sessionSubmission.StartDate),
                PeriodPassed = dateTimePassedEvaluator.HasDatePassed(sessionSubmission.EndDate.Value)
            };

            var sessionSubmissionCloses = new TimelineItemModel
            {
                PeriodDate = dateTimeFormatter.FormatEndDate(sessionSubmission.EndDate),
                PeriodPassed = dateTimePassedEvaluator.HasDatePassed(sessionSubmission.EndDate.Value)
            };

            var votingOpens = new TimelineItemModel
            {
                PeriodDate = dateTimeFormatter.FormatStartDate(voting.StartDate),
                PeriodPassed = dateTimePassedEvaluator.HasDatePassed(voting.EndDate.Value)
            };
            
            var votingCloses = new TimelineItemModel
            {
                PeriodDate = dateTimeFormatter.FormatEndDate(voting.EndDate),
                PeriodPassed = dateTimePassedEvaluator.HasDatePassed(voting.EndDate.Value)
            };
            
            var agendaAnnounced = new TimelineItemModel
            {
                PeriodDate = dateTimeFormatter.FormatStartDate(agendaPublished.StartDate),
                PeriodPassed = dateTimePassedEvaluator.HasDatePassed(registraion.StartDate)
            };
            
            var registrationOpens = new TimelineItemModel
            {
                PeriodDate = dateTimeFormatter.FormatStartDate(registraion.StartDate),
                PeriodPassed = dateTimePassedEvaluator.HasDatePassed(registraion.EndDate.Value)
            };

            var model = new TimelineModel
            {
                SessionSubmissionOpens = sessionSubmissionOpens,
                SessionSubmissionCloses = sessionSubmissionCloses,
                VotingOpens = votingOpens,
                VotingCloses = votingCloses,
                AgendaAnnounced = agendaAnnounced,
                RegistrationOpens = registrationOpens
            };
            
            return PartialView("_Timeline", model);
        }
    }
}
