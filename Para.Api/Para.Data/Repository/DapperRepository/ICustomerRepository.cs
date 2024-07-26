using Para.Data.Domain;

namespace Para.Data.Repository.DapperRepository;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomersAsync();
}