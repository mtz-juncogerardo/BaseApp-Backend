using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BaseApp.Data.Roles;

namespace BaseApp.Data.DbModels
{
    public class AuthenticationDbModel 
    {
        [Required, MaxLength(100)] public string Salt { get; set; }
        [Required, MaxLength(50)] public string Email { get; set; }
        [Required, MaxLength(300)] public string Password { get; set; }
        [DefaultValue(false)] public bool EmailValid { get; set; }
        [Required, MaxLength(36)] public string Id { get; set; }
        [Required, MaxLength(36)]public string VersionId { get; set; }
        [DefaultValue(UserRoleEnum.User)] public UserRoleEnum Role { get; set; }
        public UserDbModel UserDbModel { get; set; }
    }
}