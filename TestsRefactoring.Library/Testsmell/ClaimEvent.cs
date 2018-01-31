using System;

namespace TestsRefactoring.Library.TestSmell
{
    public class ClaimEvent
    {
        public const string CreatedEvent = "Created";
        public const string DeletedEvent = "Created";
        
        public string Event { get; set; }
        public string Predicate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Identifier { get; set; }
        public string Source { get; set; }
    }
}