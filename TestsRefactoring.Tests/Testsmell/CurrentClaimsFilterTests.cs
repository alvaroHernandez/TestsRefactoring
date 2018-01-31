using System;
using System.Linq;
using Moq;
using TestsRefactoring.Library.TestSmell;
using TestsRefactoring.Tests.Builders;
using TestsRefactoring.Tests.Extensions;
using Xunit;

namespace TestsRefactoring.Tests.Testsmell
{
    public class CurrentClaimsFilterTests
    {
        
        // Current claim: created and not be deleted for another claim created after it, and its date have to be after given threshold      
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
                ClaimEventBuilder.New().With(c => c.CreatedDate = DateTime.Parse("2007-01-01")).Build()
            };
            
            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims.AsQueryable());

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");            

            Assert.Empty(claimFilter.Filter());
        }

        [Fact]
        public void ShouldReturnClaimsWithSamePredicateAndSourceAsOneClaim()
        {
            var claim = ClaimEventBuilder.New().Build();
            var similarClaim = ClaimEventBuilder.New()
                .With(c => c.Predicate = claim.Predicate)
                .With(c => c.Source = claim.Source)
                .Build();
            
            var existingClaims = new[]
            {
                claim,
                similarClaim
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
            var claim = ClaimEventBuilder.New().Build();
            var latestClaim = ClaimEventBuilder.New()
                .With(c => c.Predicate = claim.Predicate)
                .With(c => c.Source = claim.Source)
                .With(c => c.CreatedDate = claim.CreatedDate.AddSeconds(1))
                .Build();
            
            var existingClaims = new[]
            {
                claim,
                latestClaim
            };
            
            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims.Shuffle().AsQueryable());

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");            

            var result = claimFilter.Filter();
            Assert.Single(result);
            Assert.Equal(latestClaim, result.First());
        }

        [Fact]
        public void ShouldReturnEmptyWhenClaimWithSamePredicateAndSourceIsCreatedLaterWithDeletedEvent()
        {
            var claim = ClaimEventBuilder.New().With(c => c.Event = "Created").Build();
            var latestClaim = ClaimEventBuilder.New()
                .With(c => c.Predicate = claim.Predicate)
                .With(c => c.Source = claim.Source)
                .With(c => c.CreatedDate = claim.CreatedDate.AddSeconds(1))
                .With(c => c.Event = "Deleted")
                .Build();
            
            var existingClaims = new[]
            {
                claim,
                latestClaim
            };
            
            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(existingClaims.Shuffle().AsQueryable());

            var claimFilter = new CurrentClaimsFilter(claimRepo.Object,"2008-01-01");            

            var result = claimFilter.Filter();
            Assert.Empty(result);   
        }
    }
}