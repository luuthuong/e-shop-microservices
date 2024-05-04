using Core.CQRS.Query;
using CustomerService.DTO.Customers;
using Domain;

namespace Application.Queries;

public record GetCustomerByIdQuery(CustomerId CustomerId) : IQuery<CustomerDTO>; 