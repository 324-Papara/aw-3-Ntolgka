using System.Data;
using Dapper;
using Para.Data.Domain;

namespace Para.Data.Repository.DapperRepository;
//TODO - Register this repository
public class CustomerRepository : ICustomerRepository
{
    private readonly IDbConnection _dbConnection;

    public CustomerRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        //Change the sql depending on your Schema in Database.
        const string sql = @"
            SELECT c.id AS customer_id, c.*, cd.*, ca.*, cp.*
            FROM dbo.Customer c
            LEFT JOIN dbo.CustomerDetail cd ON c.id = cd.CustomerId
            LEFT JOIN dbo.CustomerAddress ca ON c.id = ca.CustomerId
            LEFT JOIN dbo.CustomerPhone cp ON c.id = cp.CustomerId;
        ";
        
        var customerDict = new Dictionary<long, Customer>();

        var customers = await _dbConnection.QueryAsync<Customer, CustomerDetail, CustomerAddress, CustomerPhone, Customer>(
            sql,
            (customer, detail, address, phone) =>
            {
                if (!customerDict.TryGetValue(customer.Id, out var currentCustomer))
                {
                    currentCustomer = customer;
                    currentCustomer.CustomerDetails = detail;
                    currentCustomer.CustomerAddresses = new List<CustomerAddress>();
                    currentCustomer.CustomerPhones = new List<CustomerPhone>();
                    customerDict[customer.Id] = currentCustomer;
                }

                if (address != null && !currentCustomer.CustomerAddresses.Contains(address))
                {
                    currentCustomer.CustomerAddresses.Add(address);
                }

                if (phone != null && !currentCustomer.CustomerPhones.Contains(phone))
                {
                    currentCustomer.CustomerPhones.Add(phone);
                }

                return currentCustomer;
            },
            splitOn: "customer_id,CustomerId,CustomerId");

        return customers.Distinct().ToList();
    }
        
}