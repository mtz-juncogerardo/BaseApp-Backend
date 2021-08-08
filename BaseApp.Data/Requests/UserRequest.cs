using BaseApp.Data.DbModels.JoinedModels;

namespace BaseApp.Data.Requests
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public AuthenticationUserAuditModel ConvertToDbModel(AuthenticationUserAuditModel authUserAudit)
        {
            authUserAudit.UserDb.Name = Name;
            authUserAudit.UserDb.Phone = Phone;
            authUserAudit.UserDb.Address = Address;
            authUserAudit.UserDb.City = City;
            authUserAudit.UserDb.State = State;
            authUserAudit.UserDb.PostalCode = PostalCode;

            return authUserAudit;
        }
    }
}