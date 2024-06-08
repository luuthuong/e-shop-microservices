using Core.Infrastructure.EF.Repository;
using Domain;

namespace CustomerService.Infrastructure.Persistence;

public class CustomerRepository(CustomerDbContext dbContext)
    : Repository<CustomerDbContext, Customer, CustomerId>(dbContext), ICustomerRepository;