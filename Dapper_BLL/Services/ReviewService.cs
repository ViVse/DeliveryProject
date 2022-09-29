using AutoMapper;
using Dapper_BLL.DTO.Requests;
using Dapper_BLL.DTO.Responses;
using Dapper_BLL.Interfaces;
using Dapper_DAL.Entites;
using Dapper_DAL.Interfaces;

namespace Dapper_BLL.Services
{
    public class ReviewService: IReviewService
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IMapper mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            this.reviewRepository = reviewRepository;
            this.mapper = mapper;
        }

        public async Task DeleteAsync(int id)
        {
            await reviewRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ReviewResponse>> GetAsync()
        {
            var reviews = await reviewRepository.GetAsync();
            return reviews?.Select(mapper.Map<Review, ReviewResponse>);
        }

        public async Task<ReviewResponse> GetByIdAsync(int id)
        {
            var review = await reviewRepository.GetByIdAsync(id);
            return mapper.Map<Review, ReviewResponse>(review);
        }

        public async Task InsertAsync(ReviewRequest request)
        {
            var review = mapper.Map<ReviewRequest, Review>(request);
            await reviewRepository.InsertAsync(review);
        }

        public async Task UpdateAsync(ReviewRequest request)
        {
            var review = mapper.Map<ReviewRequest, Review>(request);
            await reviewRepository.UpdateAsync(review);
        }
    }
}

