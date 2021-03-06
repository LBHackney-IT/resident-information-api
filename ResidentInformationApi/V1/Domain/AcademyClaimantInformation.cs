using System.Collections.Generic;

namespace ResidentInformationApi.V1.Domain
{

    public class AcademyClaimantResponse
    {
        public List<AcademyClaimantInformation> Claimants { get; set; }

        public string NextCursor { get; set; }
    }
    public class AcademyClaimantInformation
    {
        public int ClaimId { get; set; }
        public int PersonRef { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string NINumber { get; set; }
        public Address ClaimantAddress { get; set; }
        public string CheckDigit { get; set; }
    }
}
