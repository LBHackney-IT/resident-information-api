using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidentInformationApi.V1.Domain
{
    public class ElectoralRegisterResidentInformation
    {
        public int ElectoralRegisterId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Uprn { get; set; }
    }
}
