using ResidentInformationApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidentInformationApi.V1.Boundary.Responses
{
    public class ElectoralRegisterResidentResponse : IResidentData
    {
        /// <example>
        /// abc123
        /// </example>
        public string ElectoralRegisterId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Uprn { get; set; }
    }
    public class ElectoralRegisterResidentsResponse
    {
        public List<ElectoralRegisterResidentInformation> Residents { get; set; }

        public string NextCursor { get; set; }
    }
}
