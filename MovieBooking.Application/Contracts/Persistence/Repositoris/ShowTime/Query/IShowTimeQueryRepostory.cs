﻿using Ardalis.Specification;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.ShowTimes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Persistence.Repositoris.ShowTime.Query
{
    public interface IShowTimeQueryRepostory : IQueryRepository<Domain.Entities.Showtime>
    {
        Task<IPagedDataResponse<ShowTimeListDto>> SearchAsync(ISpecification<ShowTimeListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);

        List<ShowTimeDetailDto> GetAvailableShowTimes(Guid movieId, Guid theaterId);
        Task<List<MovieBooking.Application.Features.ShowTimes.Dto.ShowTimeMainDetail>> GetAllShowtimeMainAsync(CancellationToken cancellationToken);

        List<ShowTimeDetailDto> GetAvailableShowTimesForScreen(Guid movieId, Guid theaterId, Guid screenId);
        //Task<List<MovieBooking.Application.Features.ShowTimes.Dto.ShowTimeMainDetail>> GetAllShowtimeMainAsync(CancellationToken cancellationToken);

    }
}
