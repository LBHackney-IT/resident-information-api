using System.Collections.Generic;

namespace ResidentInformationApi.V1.Boundary.Responses
{
    public class MosaicResidentInformationList
    {
        public List<MosaicResidentInformation> Residents { get; set; }

        public string NextCursor { get; set; }

    }
}
