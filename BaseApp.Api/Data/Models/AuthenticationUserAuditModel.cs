using BaseApp.Data.DbModels;

namespace BaseApp.Data.Models
{
    public class AuthenticationUserAuditModel
    {
        public UserDbModel UserDb { get; set; }
        public AuthenticationDbModel AuthenticationDb { get; set; }
        public AuditDbModel AuditDb { get; set; }
    }
}