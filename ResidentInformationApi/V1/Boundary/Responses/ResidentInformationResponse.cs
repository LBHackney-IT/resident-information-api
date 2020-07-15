using System;
using System.Collections.Generic;

namespace ResidentInformationApi.V1.Boundary.Responses
{
    public class ResidentInformationResponse
    {
        public List<ResidentInformationResult> Results { get; set; }
    }

    public class ResidentInformationResult
    {
        /// <example>
        /// academy
        /// </example>
        public string System { get; set; }
        /// <example>
        /// 123_456
        /// </example>
        public string SystemId { get; set; }
        /// <example>
        /// https://academy-api.hackney.gov.uk
        /// </example>
        public Uri SystemUrl { get; set; }
        public IResidentData Data { get; set; }
    }

    public interface IResidentData
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string DateOfBirth { get; set; }
    }
}
