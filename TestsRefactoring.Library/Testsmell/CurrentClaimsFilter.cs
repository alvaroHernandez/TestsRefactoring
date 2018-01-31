using System;
using System.Collections.Generic;

namespace TestsRefactoring.Library.TestSmell
{
    public class CurrentClaimsFilter
    {
        public DateTime DateThreshold { get; }

        public CurrentClaimsFilter(ClaimRepository claimRepoObject, string dateThreshold)
        {
            DateThreshold = DateTime.Parse(dateThreshold);
        }

        public List<ClaimEvent> Filter()
        {
            return null;
        }
    }
}