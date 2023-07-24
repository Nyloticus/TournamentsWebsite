using Domain.Attributes;

namespace Domain.Enums
{
    public enum SettingKeys
    {
        [DescribeSetting("1", "session_expiry", SettingType.Number, "8")]
        SessionExpiry,
        [DescribeSetting("2", "outlet_max_distance", SettingType.Number, "100")]
        OutletPromoterMaxDistance,
    }
}