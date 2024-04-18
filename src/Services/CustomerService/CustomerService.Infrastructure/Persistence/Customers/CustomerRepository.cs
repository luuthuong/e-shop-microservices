using Core.Infrastructure.EF.Repository;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence.Customers;

public class CustomerRepository: Repository<CustomerDbContext,Customer>,ICustomerRepository
{
    public CustomerRepository(CustomerDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Customer?> GetByIdAsync(CustomerId customerId, CancellationToken cancellationToken)
    {
        return DBSet.FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
    }
}