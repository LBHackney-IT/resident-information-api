using System.Collections.Generic;

namespace ResidentInformationApi.V1.Boundary.Responses
{
    public class ResidentInformationList
    {
        public List<ResidentInformation> Residents { get; }

        public string NextCursor { get; set; }
    }
}
