namespace BaseApp.Data.DbModels.JoinedModels
{
    public class AuthenticationUserAuditModel : IDbModel
    {
        public UserDbModel UserDb { get; set; }
        public AuthenticationDbModel AuthenticationDb { get; set; }
        public AuditDbModel AuditDb { get; set; }
        
        public string Id { get; set; }
    }
}