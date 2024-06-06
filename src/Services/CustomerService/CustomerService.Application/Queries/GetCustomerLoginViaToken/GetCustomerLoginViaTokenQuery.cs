using Core.CQRS.Query;
using CustomerService.DTO.Customers;

namespace Application.Queries;

public sealed record GetCustomerLoginViaTokenQuery() : IQuery<CustomerLoginDTO?>;
