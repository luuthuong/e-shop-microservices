using Core.EF;

namespace Domain.Customers;

public interface ICustomerRepository: IRepository<Customer>
{
    Task<Customer?> GetByIdAsync(CustomerId customerId, CancellationToken cancellationToken);
}