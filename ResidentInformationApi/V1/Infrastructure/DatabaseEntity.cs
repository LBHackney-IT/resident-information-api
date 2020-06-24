using System.ComponentModel.DataAnnotations.Schema;

namespace ResidentInformationApi.V1.Infrastructure
{
    [Table("example_entities")]
    public class DatabaseEntity
    {
        [Column("id")]
        public int Id { get; set; }
    }
}
