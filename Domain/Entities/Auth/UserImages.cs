using Common;

namespace Domain.Entities.Auth
{
    public class UserImages : BaseEntityAudit<string>/*, ITenant*/
    {
        // public string TenantId { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
