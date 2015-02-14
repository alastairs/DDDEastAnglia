﻿using System;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData
{
    public class VoteRepository : IVoteRepository
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public void AddVote(Vote vote)
        {
            db.Votes.Insert(vote);
        }

        public void Delete(int sessionId, Guid cookieId)
        {
            db.Votes.Delete(SessionId : sessionId, cookieId : cookieId);
        }

        public bool HasVotedFor(int sessionId, Guid cookieId)
        {
            return db.Votes.Exists(db.Votes.CookieId == cookieId && db.Votes.SessionId == sessionId);
        }
    }
}
