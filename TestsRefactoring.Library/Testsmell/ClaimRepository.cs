using System.Collections.Generic;
using System.Linq;

namespace TestsRefactoring.Library.TestSmell
{
    public class ClaimRepository
    {
        public virtual IQueryable<ClaimEvent> Query()
        {
            return null;
        }
    }
}