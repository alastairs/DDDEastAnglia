using System.Collections.Generic;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public class AllVotesQuery
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<Vote> Execute()
        {
            return db.Votes.All();
        } 
    }
}