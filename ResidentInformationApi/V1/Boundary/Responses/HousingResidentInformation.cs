using System.Collections.Generic;

namespace ResidentInformationApi.V1.Boundary.Responses
{
    public class HousingResidentInformation : IResidentData
    {
        public string HouseReference { get; set; }
        public string PersonNumber { get; set; }
        public string TenancyReference { get; set; }
        public string NationalInsuranceNumber { get; set; }

        /// <example>
        /// Ciasom
        /// </example>
        public string FirstName { get; set; }
        /// <example>
        /// Tessellate
        /// </example>
        public string LastName { get; set; }
        /// <example>
        /// 1000000000
        /// </example>
        public string Uprn { get; set; }
        /// <example>
        /// 2020-05-15
        /// </example>
        public string DateOfBirth { get; set; }
        public List<Phone> PhoneNumber { get; set; }
        public Address Address { get; set; }
        /// <example>
        /// 450 557 7104
        /// </example>
        public List<Email> EmailAddressList { get; set; }
        public string NhsNumber { get; set; }
    }
}
