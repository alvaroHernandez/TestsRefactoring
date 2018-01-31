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
            var claimFilter = new CurrentClaimsFilter(null,DateTime.Today.ToString());
            Assert.Equal(claimFilter.DateThreshold,DateTime.Today);
        }

        [Fact]
        public void ShouldReturnEmptyWhenClaimIsBeforeGivenDateThreshold()
        {
            var ignoredClaim = ClaimEventBuilder.New().Build();            
            var claimRepo = CreateClaimRepositoryWithStubbedClaims(new []{ignoredClaim});
            
            var dateThreshold = ignoredClaim.CreatedDate.AddDays(1).ToString();
            var claimFilter = new CurrentClaimsFilter(claimRepo,dateThreshold);            

            Assert.Empty(claimFilter.Filter());
        }

        [Fact]
        public void ShouldReturnClaimsWithSamePredicateAndSourceAsOneClaim()
        {
            var claim = ClaimEventBuilder.New().Build();
            var similarClaim = ClaimEventBuilder.New()
                .WithSamePredicateAndSourceThan(claim)
                .Build();
            
            var claimRepo = CreateClaimRepositoryWithStubbedClaims(new []{claim,similarClaim});
            var claimFilter = CreateCurrentClaimFilterWithRepository(claimRepo);            

            var result = claimFilter.Filter();
            
            Assert.Single(result);            
        }

        [Fact]
        public void ShouldReturnMostRecentClaimWhenTheyHaveSamePredicateAndSource()
        {
            var claim = ClaimEventBuilder.New().Build();
            var latestClaim = ClaimEventBuilder.New()
                .WithSamePredicateAndSourceThan(claim)
                .WithCreatedDateLaterThan(claim).Build();
            
            var claimRepo = CreateClaimRepositoryWithStubbedClaims(new []{claim,latestClaim});

            var claimFilter = new CurrentClaimsFilter(claimRepo,DateTime.MinValue.ToString());           

            var result = claimFilter.Filter();
            
            Assert.Single(result);
            Assert.Equal(latestClaim, result.First());
        }

        [Fact]
        public void ShouldReturnEmptyWhenClaimWithSamePredicateAndSourceIsCreatedLaterWithDeletedEvent()
        {
            var creationClaim = ClaimEventBuilder.NewCreatedClaimEvent().Build();
            var deletionClaim = ClaimEventBuilder.NewDeletedClaimEvent()
                .WithSamePredicateAndSourceThan(creationClaim)
                .WithCreatedDateLaterThan(creationClaim)
                .Build();
            
            var claimRepo = CreateClaimRepositoryWithStubbedClaims(new []{creationClaim, deletionClaim});
            var claimFilter = CreateCurrentClaimFilterWithRepository(claimRepo);               
            
            var result = claimFilter.Filter();
            
            Assert.Empty(result);   
        }
        
        private CurrentClaimsFilter CreateCurrentClaimFilterWithRepository(ClaimRepository claimsRepository)
        {
            return new CurrentClaimsFilter(claimsRepository, DateTime.MinValue.ToString());
        }

        private static ClaimRepository CreateClaimRepositoryWithStubbedClaims(ClaimEvent[] stubbedClaims)
        {
            var claimRepo = new Mock<ClaimRepository>();
            claimRepo.Setup(c => c.Query()).Returns(stubbedClaims.Shuffle().AsQueryable());
            return claimRepo.Object;
        }
    }
}