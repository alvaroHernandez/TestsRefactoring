using System;

namespace TestsRefactoring.Library.TestSmell
{
    public class ClaimEvent
    {
        public string Event { get; set; }
        public string Predicate { get; set; }
        public DateTime Timestamp { get; set; }
        public int Identifier { get; set; }
        public string ClaimSource { get; set; }
    }
}