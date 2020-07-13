using ResidentInformationApi.V1.Boundary.Responses;

namespace ResidentInformationApi.V1.Domain
{
    public class PhoneNumber
    {
        public string Number { get; set; }
        public PhoneType Type { get; set; }
    }
}
