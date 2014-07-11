using System;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IVoteRepository
    {
        void AddVote(Vote vote);
        void Delete(int sessionId, Guid cookieId);
        bool HasVotedFor(int sessionId, Guid cookieId);
    }
}