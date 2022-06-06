using AutoMapper;
using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Interfaces.Services;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class ReviewService: IReviewService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.ReviewRepository.DeleteAsync(id);
            await unitOfWork.Commit();
        }

        public async Task<IEnumerable<ReviewResponse>> GetAsync()
        {
            var reviews = await unitOfWork.ReviewRepository.GetAllAsync();
            return reviews?.Select(mapper.Map<Review, ReviewResponse>);
        }

        public async Task<ReviewResponse> GetByIdAsync(int id)
        {
            var review = await unitOfWork.ReviewRepository.GetByIdAsync(id);
            return mapper.Map<Review, ReviewResponse>(review);
        }

        public async Task InsertAsync(ReviewRequest request)
        {
            var review = mapper.Map<ReviewRequest, Review>(request);
            await unitOfWork.ReviewRepository.InsertAsync(review);
            await unitOfWork.Commit();
        }

        public async Task UpdateAsync(ReviewRequest request)
        {
            var review = mapper.Map<ReviewRequest, Review>(request);
            await unitOfWork.ReviewRepository.UpdateAsync(review);
            await unitOfWork.Commit();
        }
    }
}
