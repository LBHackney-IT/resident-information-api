using System.Collections.Generic;
using ResidentInformationApi.V1.Boundary.Responses;

namespace ResidentInformationApi.V1.Domain
{
    public class HousingResidentInformation
    {
        public string HouseReference { get; set; }
        public string PersonNumber { get; set; }
        public string TenancyReference { get; set; }
        public string NationalInsuranceNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Uprn { get; set; }
        public string DateOfBirth { get; set; }
        public string NhsNumber { get; set; }
        public List<PhoneNumber> PhoneNumberList { get; set; }
        public List<Address> AddressList { get; set; }
        public List<Email> EmailAddressList { get; set; }
    }

    public class PhoneNumber
    {
        public string Number { get; set; }
        public PhoneType Type { get; set; }
    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostCode { get; set; }
    }

    public class Email
    {
        public string EmailAddress { get; set; }
        public string DateLastModified { get; set; }
    }
}
