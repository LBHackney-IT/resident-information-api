using System.Collections.Generic;

namespace ResidentInformationApi.V1.Domain
{
    public class MosaicResidentInformation
    {
        public string MosaicId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Uprn { get; set; }
        public string DateOfBirth { get; set; }
        public List<PhoneNumber> PhoneNumberList { get; set; }
        public List<Address> AddressList { get; set; }
        public string NhsNumber { get; set; }
    }
}
