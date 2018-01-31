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
            .With(c => c.Event = GenerateRandomValidEvent(random))
            .With(c => c.Identifier = random.Next())
            .With(c => c.Source = Guid.NewGuid().ToString())
            .With(c => c.CreatedDate = DateTime.Now)
            .With(c => c.Predicate = Guid.NewGuid().ToString());
        }
        
        private static string GenerateRandomValidEvent(Random random)
        {
            return random.Next(0, 2) > 0 ? ClaimEvent.CreatedEvent : ClaimEvent.DeletedEvent;
        }

        public static ClaimEventBuilder New()
        {
            return new ClaimEventBuilder();
        }
    }
}