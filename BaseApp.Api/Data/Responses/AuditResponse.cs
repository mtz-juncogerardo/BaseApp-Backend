using System;
using BaseApp.Data.DbModels;

namespace BaseApp.Data.Responses
{
    public class AuditResponse
    {
        public AuditResponse(AuditDbModel audit)
        {
            CreatedAt = audit.CreatedAtDate;
            UpdatedAt = audit.UpdatedAtDate;
            CreatedByEmail = audit.CreatedByUserEmail;
            UpdatedByEmail = audit.UpdatedByUserEmail;
            UpdatedByIpAddress= audit.LastTouchedByIp;
        }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
        public string CreatedByEmail { get; }
        public string UpdatedByEmail { get; }
        public string UpdatedByIpAddress { get; }
    }
}