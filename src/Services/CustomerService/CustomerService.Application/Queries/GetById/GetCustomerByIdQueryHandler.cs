using Core.CQRS.Query;
using CustomerService.DTO.Customers;

namespace Application.Queries;

internal sealed class GetCustomerByIdQueryHandler: IQueryHandler<GetCustomerByIdQuery, CustomerDTO>
{
    public Task<CustomerDTO> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return default;
    }
}