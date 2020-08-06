using System.Collections.Generic;

namespace ResidentInformationApi.V1.Domain
{
    public class HousingApiResponse
    {
        public List<HousingResidentInformation> Residents { get; set; }

        public string NextCursor { get; set; }
    }

    public class HousingResidentInformation
    {
        public string HouseReference { get; set; }
        public string PersonNumber { get; set; }
        public string TenancyReference { get; set; }
        public string NiNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Uprn { get; set; }
        public string DateOfBirth { get; set; }
        public List<Phone> PhoneNumber { get; set; }
        public Address Address { get; set; }
        public List<HousingEmail> Email { get; set; }
    }

    public class HousingEmail
    {
        public string EmailAddress { get; set; }
        public string LastModified { get; set; }
    }
}
