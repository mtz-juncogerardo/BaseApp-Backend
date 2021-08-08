using System.ComponentModel.DataAnnotations;

namespace BaseApp.Data.DbModels
{
    public class UserDbModel : IDbModel
    {
        [Required] public string AuthenticationDbModelId { get; set; }
        [Required] public string AuditDbModelId { get; set; }
        [MaxLength(36)] public string Name { get; set; }
        [MaxLength(12)] public string Phone { get; set; }
        [MaxLength(150)] public string Address { get; set; }
        [MaxLength(50)] public string City { get; set; }
        [MaxLength(50)] public string State { get; set; }
        [MaxLength(5)] public string PostalCode { get; set; }
        public AuthenticationDbModel AuthenticationDbModel { get; set; }
        [Required, MaxLength(36)] public string Id { get; set; }
        public AuditDbModel AuditDbModel { get; set; }
    }
}