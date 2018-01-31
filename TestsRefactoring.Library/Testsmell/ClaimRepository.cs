using System.Collections.Generic;

namespace TestsRefactoring.Library.TestSmell
{
    public class ClaimRepository
    {
        public virtual IEnumerable<ClaimEvent> Query()
        {
            return null;
        }
    }
}