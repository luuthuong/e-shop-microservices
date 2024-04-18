using Core.CQRS.Query;
using CustomerService.DTO.Customers;
using Domain.Customers;

namespace Application.Customers;

public record GetCustomerByIdQuery(CustomerId CustomerId) : IQuery<CustomerDTO>; 