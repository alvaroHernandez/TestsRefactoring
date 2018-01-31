using System;
using System.Collections.Generic;
using System.Linq;

namespace TestsRefactoring.Library.TestSmell
{
    public class CurrentClaimsFilter
    {
        private readonly ClaimRepository _claimRepository;
        public DateTime DateThreshold { get; }

        public CurrentClaimsFilter(ClaimRepository claimRepository, string dateThreshold)
        {
            _claimRepository = claimRepository;
            DateThreshold = DateTime.Parse(dateThreshold);
        }

        public List<ClaimEvent> Filter()
        {
            var claims = _claimRepository.Query();
            return claims.Where(c => DateTime.Compare(c.Timestamp,DateThreshold) >= 0).GroupBy(c => new { c.Predicate, c.ClaimSource})
                .Select(group => group.OrderByDescending(c => c.Timestamp).First())
                .Where(c => c.Event == "Created").ToList();
        }
    }
}