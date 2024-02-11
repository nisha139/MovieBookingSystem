using Ardalis.Specification;
using MovieBooking.Application.Models.Specification;
using MovieBooking.Application.Models.Specification.Filters;

namespace MovieBooking.Application.Models.Users;
public class UserListFilter : PaginationFilter
{
}

public class GetSearchUserRequestSpec : EntitiesByPaginationFilterSpec<UserListDto>
{
    public GetSearchUserRequestSpec(UserListFilter request)
    : base(request) =>
        Query.OrderByDescending(c => c.Id, !request.HasOrderBy());
}
