using BaseApp.Data.DbModels;
using BaseApp.Data.Models;

namespace BaseApp.Data.Responses
{
    public class UserResponse
    {
        public UserResponse(AuthenticationUserAuditModel authUserAudit)
        {
            Id =authUserAudit.UserDb.Id;
            Name =authUserAudit.UserDb.Name;
            Email =authUserAudit.AuthenticationDb.Email;
            Phone =authUserAudit.UserDb.Phone;
            Address =authUserAudit.UserDb.Address;
            City =authUserAudit.UserDb.City;
            State =authUserAudit.UserDb.State;
            PostalCode =authUserAudit.UserDb.PostalCode;
            AuditDbFields = new AuditResponse(authUserAudit.AuditDb);
        }
        
        public string Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public string Address { get; }
        public string City { get; }
        public string State { get; }
        public string PostalCode { get; }
        public AuditResponse AuditDbFields { get; }
    }
}