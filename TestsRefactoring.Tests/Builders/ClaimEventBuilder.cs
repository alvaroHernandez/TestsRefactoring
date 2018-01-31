using System;
using TestsRefactoring.Library.TestSmell;

namespace TestsRefactoring.Tests.Builders
{
    public class ClaimEventBuilder : Builder<ClaimEvent>
    {
        private ClaimEventBuilder()
        {
            var random = new Random();
            this
            .With(c => c.Event = Guid.NewGuid().ToString())
            .With(c => c.Identifier = random.Next())
            .With(c => c.Source = Guid.NewGuid().ToString())
            .With(c => c.CreatedDate = DateTime.Now)
            .With(c => c.Predicate = Guid.NewGuid().ToString());
        }

        public static ClaimEventBuilder New()
        {
            return new ClaimEventBuilder();
        }
    }
}