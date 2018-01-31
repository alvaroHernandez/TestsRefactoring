using System;
using System.Linq;
using Moq;
using TestsRefactoring.Library.TestSmell;
using Xunit;

namespace TestsRefactoring.Tests.Testsmell
{
    public class CurrentClaimsFilterTests
    {
        
        // Current claim: created and not be deleted for another claim created after it, and its date have to be after given threshold.gcane
        [Fact]
        public void ShouldFilterClaims()
        {
            var existingClaims = new[] { 
                new ClaimEvent
                {
                    Identifier = 1,
                    Event = "Created",
                    ClaimSource = "FakeClaimSource",
                    Predicate = "has name",
                    Timestamp = DateTime.Now
                },
                new ClaimEvent
                {
                    Identifier = 2,
                    Event = "Deleted",
                    ClaimSource = "FakeClaimSource",
                    Predicate = "has name",
                    Timestamp = DateTime.Now.AddSeconds(1)
                },
                new ClaimEvent
                {
                    Identifier = 3,
                    Event = "Created",
                    ClaimSource = "AnotherFakeClaimSource",
                    Predicate = "has address",
                    Timestamp = DateTime.Now.AddSeconds(4)
                },
                new ClaimEvent
                {
                    Identifier = 4,
                    Event = "Created",
                    ClaimSource = "AnotherOneFakeClaimSource",
                    Predicate = "has country",
                    Timestamp = DateTime.Parse("2007-01-01")
                }
            };

            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims.AsQueryable());

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");
            Assert.Equal(claimFilter.DateThreshold,DateTime.Parse("2008-01-01"));

            var result = claimFilter.Filter();
            Assert.Single(result);
            Assert.Equal(existingClaims[2], result.First());
        }    
    }
}