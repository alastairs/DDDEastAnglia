﻿using System;
using System.Linq;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Models;
using DDDEastAnglia.Models.Query;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public class BannerModelQuery : IBannerModelQuery
    {
        private readonly IConferenceLoader conferenceLoader;

        public BannerModelQuery(IConferenceLoader conferenceLoader)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }

            this.conferenceLoader = conferenceLoader;
        }

        public BannerModel Get()
        {
            var conference = conferenceLoader.LoadConference();
                
            if (conference == null)
            {
                return new BannerModel();
            }
                
            DateTimeOffset submissionCloses = DateTimeOffset.Now.AddDays(-1);
            DateTimeOffset votingCloses = DateTimeOffset.Now.AddDays(-1);

            var allDates = new AllCalendarItemsQuery().Execute().ToDictionary(c => c.EntryType, c => c);
            var submission = allDates[CalendarEntryType.SessionSubmission];
                    
            if (submission != null && submission.EndDate.HasValue)
            {
                submissionCloses = submission.EndDate.Value;
            }
                    
            var voting = allDates[CalendarEntryType.Voting];

            if (voting != null && voting.EndDate.HasValue)
            {
                votingCloses = voting.EndDate.Value;
            }

            return new BannerModel
            {
                IsOpenForSubmission = conference.CanSubmit(),
                IsOpenForVoting = conference.CanVote(),
                SessionSubmissionCloses = submissionCloses.ToString("R"),
                VotingCloses = votingCloses.ToString("R")
            };
        }
    }
}
