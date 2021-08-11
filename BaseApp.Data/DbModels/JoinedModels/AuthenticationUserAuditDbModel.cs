namespace BaseApp.Data.DbModels.JoinedModels
{
    public class AuthenticationUserAuditDbModel : IAuditDbModel 
    {
        public UserDbModel UserDb { get; set; }
        public AuthenticationDbModel AuthenticationDb { get; set; }
        public AuditDbModel AuditDb { get; set; }
    }
}