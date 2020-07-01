using System.Collections.Generic;

namespace ResidentInformationApi.V1.Boundary.Responses
{
    public class HousingResidentInformationList
    {
        public List<HousingResidentInformation> Residents { get; }

        public string NextCursor { get; set; }
    }
}
