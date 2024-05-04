using Core.EF;

namespace Domain;

public interface ICustomerRepository: IRepository<Customer, CustomerId>
{
}