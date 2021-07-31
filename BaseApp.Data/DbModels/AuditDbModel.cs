using System;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Data.DbModels
{
    public class AuditDbModel
    {
        [Required, MaxLength(36)] public string? Id { get; set; }
        [Required] public DateTime? CreatedAtDate { get; set; }
        [Required, MaxLength(50)] public string? CreatedByUserEmail { get; set; }
        [MaxLength(50)] public string? UpdatedByUserEmail { get; set; }
        [MaxLength(15)] public string? LastTouchedByIp { get; set; }
        public DateTime? UpdatedAtDate { get; set; }

        public UserDbModel? UserDbModel { get; set; }
    }
}