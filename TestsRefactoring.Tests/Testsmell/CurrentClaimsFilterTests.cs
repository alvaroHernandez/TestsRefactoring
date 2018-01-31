using System;
using System.Linq;
using Moq;
using TestsRefactoring.Library.TestSmell;
using Xunit;

namespace TestsRefactoring.Tests.Testsmell
{
    public class CurrentClaimsFilterTests
    {
        
        // Current claim: created and not be deleted for another claim created after it, and its date have to be after given threshold
        [Fact]
        public void ShouldReturnNotDeletedClaimCreatedAfterGivenDateThreshold()
        {
            var existingClaims = new[] { 
                new ClaimEvent
                {
                    Identifier = 1,
                    Event = "Created",
                    Source = "FakeSource",
                    Predicate = "has name",
                    CreatedDate = DateTime.Now
                },
                new ClaimEvent
                {
                    Identifier = 2,
                    Event = "Deleted",
                    Source = "FakeSource",
                    Predicate = "has name",
                    CreatedDate = DateTime.Now.AddSeconds(1)
                },
                new ClaimEvent
                {
                    Identifier = 3,
                    Event = "Created",
                    Source = "AnotherFakeSource",
                    Predicate = "has address",
                    CreatedDate = DateTime.Now.AddSeconds(4)
                }
            };

            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims.AsQueryable());

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");            

            var result = claimFilter.Filter();
            Assert.Single(result);
            Assert.Equal(existingClaims[2], result.First());
        }

        [Fact]
        public void ShouldAssingDateToFilterThreshold()
        {
            var claimFilter = new CurrentClaimsFilter(null,"2008-01-01");
            Assert.Equal(claimFilter.DateThreshold,DateTime.Parse("2008-01-01"));
        }

        [Fact]
        public void ShouldReturnEmptyWhenClaimIsBeforeGivenDateThreshold()
        {
            var existingClaims = new[]
            {
                new ClaimEvent
                {
                    Identifier = 4,
                    Event = "Created",
                    Source = "AnotherOneFakeSource",
                    Predicate = "has country",
                    CreatedDate = DateTime.Parse("2007-01-01")
                }
            };
            
            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims.AsQueryable());

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");            

            Assert.Empty(claimFilter.Filter());
        }

        [Fact]
        public void ShouldReturnClaimsWithSamePredicateAndSourceAsOneClaim()
        {
            var existingClaims = new[]
            {
                new ClaimEvent
                {
                    Identifier = 1,
                    Event = "Created",
                    Source = "FakeSource",
                    Predicate = "has name",
                    CreatedDate = DateTime.Now
                },
                new ClaimEvent
                {
                    Identifier = 2,
                    Event = "Created",
                    Source = "FakeSource",
                    Predicate = "has name",
                    CreatedDate = DateTime.Now.AddSeconds(1)
                }
            };
            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims.AsQueryable());

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");            

            var result = claimFilter.Filter();
            Assert.Single(result);            
        }

        [Fact]
        public void ShouldReturnMostRecentClaimWhenTheyHaveSamePredicateAndSource()
        {
            var existingClaims = new[]
            {
                new ClaimEvent
                {
                    Identifier = 1,
                    Event = "Created",
                    Source = "FakeSource",
                    Predicate = "has name",
                    CreatedDate = DateTime.Now
                },
                new ClaimEvent
                {
                    Identifier = 2,
                    Event = "Created",
                    Source = "FakeSource",
                    Predicate = "has name",
                    CreatedDate = DateTime.Now.AddSeconds(1)
                }
            };
            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims.AsQueryable());

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");            

            var result = claimFilter.Filter();
            Assert.Single(result);
            Assert.Equal(existingClaims[1], result.First());
        }

        [Fact]
        public void ShouldReturnEmptyWhenClaimWithSamePredicateAndSourceIsCreatedLaterWithDeletedEvent()
        {
            var existingClaims = new[]
            {
                new ClaimEvent
                {
                    Identifier = 1,
                    Event = "Created",
                    Source = "FakeSource",
                    Predicate = "has name",
                    CreatedDate = DateTime.Now
                },
                new ClaimEvent
                {
                    Identifier = 2,
                    Event = "Deleted",
                    Source = "FakeSource",
                    Predicate = "has name",
                    CreatedDate = DateTime.Now.AddSeconds(1)
                }
            };
            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims.AsQueryable());

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");            

            var result = claimFilter.Filter();
            Assert.Empty(result);   
        }
    }
}