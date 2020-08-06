using System.Collections.Generic;

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
        public Address Address { get; set; }
        public List<Email> EmailAddressList { get; set; }
    }
}
