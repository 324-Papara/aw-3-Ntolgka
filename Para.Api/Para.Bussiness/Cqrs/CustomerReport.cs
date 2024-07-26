using MediatR;
using Para.Base.Response;
using Para.Schema;

namespace Para.Bussiness.Cqrs;

public record GetCustomerReportQuery() : IRequest<ApiResponse<List<CustomerReport>>>;