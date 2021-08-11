namespace BaseApp.Data.DbModels.JoinedModels
{
    public class ArticleAuditDbModel : IAuditDbModel
    {
        public AuditDbModel AuditDb { get; set; }
        public ArticleDbModel ArticleDb { get; set; }
    }
}