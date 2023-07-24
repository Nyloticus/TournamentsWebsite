using Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Setting : BaseEntityAudit<string>
    {
        public string Name { get; set; }
        public SettingType Type { get; set; }
        public string Value { get; set; }

    }
}
