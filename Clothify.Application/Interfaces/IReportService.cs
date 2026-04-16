using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Report;

namespace Clothify.Application.Interfaces
{
    public interface IReportService
    {
        Task<Result<Guid>> AddAsync(CreateReportDto dto);
        Task<Result<bool>> RemoveAsync(Guid reportId);
        Task<Result<IReadOnlyList<ReportDto>>> GetAllByUserIdAsync(Guid userId);
        Task<Result<ReportDto>> GetAsync(Guid reportId);
    }
}
