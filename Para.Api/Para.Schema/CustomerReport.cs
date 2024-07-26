
namespace Para.Schema;

public class CustomerReport
{
    public int CustomerNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }   
    
    public virtual CustomerDetailResponse CustomerDetails { get; set; }
    public virtual List<CustomerPhoneResponse> CustomerPhones { get; set; }
    public virtual List<CustomerAddressResponse> CustomerAddresses { get; set; }
}