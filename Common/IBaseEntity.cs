using Common.Attributes;
using Common.Interfaces;
using System;

namespace Common
{

    public class BaseEntityAudit<T> : BaseEntity<T>, ICreateAudit, IUpdateAudit
    {
        [NotGenerated] public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [NotGenerated] public string CreatedBy { get; set; }
        [NotGenerated] public DateTime? UpdatedDate { get; set; }
        [NotGenerated] public string UpdatedBy { get; set; }
    }

    public class BaseEntity<T> : IBaseEntity
    {
        public T Id { get; set; }
    }

    public interface IBaseEntity { }
}



