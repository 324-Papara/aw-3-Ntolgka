using MediatR;
using Para.Base.Response;
using Para.Bussiness.Cqrs;
using Para.Data.Repository.DapperRepository;
using Para.Schema;

namespace Para.Bussiness.Query;

public class CustomerReportQueryHandler :
        IRequestHandler<GetCustomerReportQuery, ApiResponse<List<CustomerReport>>>
{ 
    private readonly ICustomerRepository _customerRepository;

    public CustomerReportQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ApiResponse<List<CustomerReport>>> Handle(GetCustomerReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var customers = await _customerRepository.GetCustomersAsync();
            
            if (customers == null || !customers.Any())
            {
                return new ApiResponse<List<CustomerReport>>("No data found.");
            }

            var customerReports = customers.Select(c => new CustomerReport
            {
                CustomerNumber = c.CustomerNumber,
                FirstName = c.FirstName,
                LastName = c.LastName,
                IdentityNumber = c.IdentityNumber,
                Email = c.Email,
                DateOfBirth = c.DateOfBirth,
                CustomerDetails = c.CustomerDetails != null ? new CustomerDetailResponse
                {
                    FatherName = c.CustomerDetails.FatherName,
                    MotherName = c.CustomerDetails.MotherName,
                    Occupation = c.CustomerDetails.Occupation,
                    MontlyIncome = c.CustomerDetails.MontlyIncome,
                    EducationStatus = c.CustomerDetails.EducationStatus
                } : null,
                CustomerAddresses = c.CustomerAddresses.Select(a => new CustomerAddressResponse
                {
                    AddressLine = a.AddressLine,
                    City = a.City,
                    ZipCode = a.ZipCode,
                    Country = a.Country
                }).ToList(),
                CustomerPhones = c.CustomerPhones.Select(p => new CustomerPhoneResponse
                {
                    Phone = p.Phone,
                    CountyCode = p.CountyCode
                }).ToList()
            }).ToList();

            return new ApiResponse<List<CustomerReport>>(customerReports);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CustomerReport>>("Internal server error.");
        }
    }
}

