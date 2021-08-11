using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.DbModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class ArticleDbModel : IDbModel
    {
        [Required, MaxLength(36)] public string Id { get; set; }
        [Required, MaxLength(36)] public string AuditDbModelId { get; set; }
        [Required] [MaxLength(100)] public string Name { get; set; }
        [MaxLength(300)] public string Description { get; set; }
        [Required] public decimal Price { get; set; }
        [DefaultValue(0)] public decimal Discount { get; set; }
        [Required] public decimal Total { get; set; }
        [Required] public decimal PointValue { get; set; }
        public AuditDbModel AuditDbModel { get; set; }
    }
}