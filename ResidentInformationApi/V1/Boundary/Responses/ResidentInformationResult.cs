namespace ResidentInformationApi.V1.Boundary.Responses
{
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
        public System.Uri SystemUrl { get; set; }
        public ResidentInformation ResidentInformation { get; set; }
    };
};
