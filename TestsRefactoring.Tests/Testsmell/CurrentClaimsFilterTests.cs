using System;
using System.Linq;
using Moq;
using TestsRefactoring.Library.TestSmell;
using Xunit;

namespace TestsRefactoring.Tests.Testsmell
{
    public class CurrentClaimsFilterTests
    {
        
        // Current claim: created and not be deleted for another claim created after it.
        [Fact]
        public void ShouldFilterClaims()
        {
            var existingClaims = new[] { 
                new ClaimEvent
                {
                    Identifier = 1,
                    Event = "ClaimCreated",
                    ClaimSource = "FakeClaimSource",
                    Predicate = "has name",
                    Timestamp = DateTime.Now
                },
                new ClaimEvent
                {
                    Identifier = 2,
                    Event = "ClaimDeleted",
                    ClaimSource = "AnotherFakeClaimSource",
                    Predicate = "has name",
                    Timestamp = DateTime.Now.AddSeconds(1)
                },
                new ClaimEvent
                {
                    Identifier = 3,
                    Event = "ClaimDeleted",
                    ClaimSource = "AnotherFakeClaimSource",
                    Predicate = "has address",
                    Timestamp = DateTime.Now.AddSeconds(1)
                },
                new ClaimEvent
                {
                    Identifier = 4,
                    Event = "ClaimCreated",
                    ClaimSource = "AnotherOneFakeClaimSource",
                    Predicate = "has address",
                    Timestamp = DateTime.Now.AddSeconds(1)
                },
                new ClaimEvent
                {
                    Identifier = 4,
                    Event = "ClaimCreated",
                    ClaimSource = "AnotherOneFakeClaimSource",
                    Predicate = "has address",
                    Timestamp = DateTime.Parse("2007-01-01")
                }
            };

            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims);

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");
            Assert.Equal(claimFilter.DateThreshold,DateTime.Parse("2008-01-01"));

            var result = claimFilter.Filter();
            Assert.Single(result);
            Assert.Equal(existingClaims[0], result.First());
        }
    }
}