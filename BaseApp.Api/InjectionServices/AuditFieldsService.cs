using System;
using BaseApp.Data.DbModels;

namespace BaseApp.InjectionServices
{
    public class AuditFieldsService : IAuditService
    {

        public AuditDbModel GetNewAuditFields(string emailCreator, string ipCreator)
        {
            return new()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAtDate = DateTime.Now,
                CreatedByUserEmail = emailCreator,
                LastTouchedByIp = ipCreator
            };
        }

        public AuditDbModel UpdateAuditFields(AuditDbModel previousAuditFields, string emailUpdater, string ipUpdater)
        {
            previousAuditFields.UpdatedAtDate = DateTime.Now;
            previousAuditFields.UpdatedByUserEmail = emailUpdater;
            previousAuditFields.LastTouchedByIp = ipUpdater;

            return previousAuditFields;
        }
    }
}