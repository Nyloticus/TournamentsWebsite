using Domain.Enums;
using System;

namespace Domain.Attributes
{
    public class DescribeSettingAttribute : Attribute
    {

        public string Id { get; set; }
        public string Key { get; set; }
        public SettingType Type { get; set; }
        public string DefaultValue { get; set; }

        public DescribeSettingAttribute(string id, string key, SettingType type, string defaultValue)
        {
            Id = id;
            Type = type;
            DefaultValue = defaultValue;
            Key = key;
        }
    }
}