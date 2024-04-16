using Core.CQRS.Query;
using CustomerService.DTO.Customers;
using Domain.Customers;

namespace Application.Queries.Customers;

public record GetCustomerByIdQuery(CustomerId CustomerId) : IQuery<CustomerDTO>; 