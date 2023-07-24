using Common;

namespace Domain.Entities.Models
{
    public class Player : BaseEntityAudit<string>
    {
        public string Name { get; set; }
        public int No { get; set; }

        //Nav props
        public string TeamId { get; set; }
        public Team Team { get; set; }
    }
}
