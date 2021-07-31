using BaseApp.Data.DbModels;

namespace BaseApp.InjectionServices
{
    public interface IAuditService
    {
        AuditDbModel GetNewAuditFields(string emailCreator, string ipCreator);
        AuditDbModel UpdateAuditFields(AuditDbModel previousAuditFields, string emailUpdater, string ipUpdater);
    }
}