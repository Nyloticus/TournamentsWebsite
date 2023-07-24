using Common;

namespace Domain.Entities.Auth
{
    public class UserDevice : BaseEntityAudit<string>, IBaseEntity
    {
        public string DeviceToken { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
