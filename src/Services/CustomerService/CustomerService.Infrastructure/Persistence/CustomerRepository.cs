using Core.Infrastructure.EF.Repository;
using Domain;

namespace CustomerService.Infrastructure.Persistence;

public class CustomerRepository: Repository<CustomerDbContext,Customer, CustomerId>,ICustomerRepository
{
    public CustomerRepository(CustomerDbContext dbContext) : base(dbContext)
    {
    }
}