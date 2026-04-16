using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Report;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(CreateReportDto dto)
        {
            var report = _mapper.Map<Report>(dto);
            report.GeneratedAt = DateTime.UtcNow;

            var added = await _unitOfWork.Reports.AddAsync(report);
            if (!added)
                return Result<Guid>.Fail("Failed to add report");

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(report.ReportId);
        }

        public async Task<Result<bool>> RemoveAsync(Guid reportId)
        {
            var report = await _unitOfWork.Reports.GetSingleEntityAsync(
                filter: r => r.ReportId == reportId
            );

            if (report is null)
                return Result<bool>.Fail("Report not found");

            var deleted = _unitOfWork.Reports.Delete(report);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete report");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<ReportDto>>> GetAllByUserIdAsync(Guid userId)
        {
            var reports = await _unitOfWork.Reports.GetAllEntitiesAsync(
                filter: r => r.UserId == userId,
                orderBy: q => q.OrderByDescending(r => r.GeneratedAt),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<ReportDto>>(reports);
            return Result<IReadOnlyList<ReportDto>>.Ok(result);
        }

        public async Task<Result<ReportDto>> GetAsync(Guid reportId)
        {
            var report = await _unitOfWork.Reports.GetSingleEntityAsync(
                filter: r => r.ReportId == reportId,
                disableTracking: true
            );

            if (report is null)
                return Result<ReportDto>.Fail("Report not found");

            var dto = _mapper.Map<ReportDto>(report);
            return Result<ReportDto>.Ok(dto);
        }
    }
}
