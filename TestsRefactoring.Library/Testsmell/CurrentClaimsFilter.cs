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
            return _claimRepository.Query()
                .Where(c => DateTime.Compare(c.CreatedDate,DateThreshold) >= 0)
                .GroupBy(c => new { c.Predicate, ClaimSource = c.Source})
                .Select(group => group.OrderByDescending(c => c.CreatedDate).First())
                .Where(c => c.Event == "Created").ToList();
        }
    }
}