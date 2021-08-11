using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BaseApp.Data.DbModels;
using BaseApp.Data.DbModels.JoinedModels;

namespace BaseApp.Data.Requests
{
    public class ArticleRequest
    {
        public string Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public decimal Price { get; set; }
        [Required] public decimal Discount { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public decimal PointValue { get; set; }

        public ArticleAuditDbModel UpdateDbModelValues(ArticleAuditDbModel articleAudit)
        {
            articleAudit.ArticleDb.Id = Id;
            articleAudit.ArticleDb.Name = Name;
            articleAudit.ArticleDb.Price = Price;
            articleAudit.ArticleDb.Discount = Discount;
            articleAudit.ArticleDb.Total = Price - Discount;
            articleAudit.ArticleDb.PointValue = Total * 0.01M;
            return articleAudit;
        }
    } 
}