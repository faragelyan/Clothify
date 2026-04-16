using AutoMapper;
using Clothify.Application.DTOs.Report;
using Clothify.Application.DTOs.Review;
using DomainReport = Clothify.Domain.Entities.Report;
using DomainReview = Clothify.Domain.Entities.Review;

namespace Clothify.Application.Mapping
{
    public class ReportReviewProfile : Profile
    {
        public ReportReviewProfile()
        {
            CreateMap<ReportDto, DomainReport>().ReverseMap();
            CreateMap<CreateReportDto, DomainReport>().ReverseMap();

            CreateMap<ReviewDto, DomainReview>().ReverseMap();
            CreateMap<CreateReviewDto, DomainReview>().ReverseMap();
            CreateMap<UpdateReviewDto, DomainReview>().ReverseMap();
        }
    }
}
