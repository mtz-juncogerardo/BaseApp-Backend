using BaseApp.Data.DbModels;
using BaseApp.Data.DbModels.JoinedModels;

namespace BaseApp.Data.Responses
{
    public class ArticleResponse
    {
        public ArticleResponse(ArticleAuditDbModel articleAudit)
        {
            Id = articleAudit.ArticleDb.Id;
            Description = articleAudit.ArticleDb.Description;
            Name = articleAudit.ArticleDb.Name;
            Price = articleAudit.ArticleDb.Price;
            Discount = articleAudit.ArticleDb.Discount;
            Total = articleAudit.ArticleDb.Price - articleAudit.ArticleDb.Discount;
            AuditDb = new AuditResponse(articleAudit.AuditDb);
        }
        
        public AuditResponse AuditDb { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}