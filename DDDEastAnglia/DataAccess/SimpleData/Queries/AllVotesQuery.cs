using System.Collections.Generic;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public class AllVotesQuery : IVectorQuery<Vote>
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<Vote> Execute()
        {
            return db.Votes.All();
        } 
    }
}