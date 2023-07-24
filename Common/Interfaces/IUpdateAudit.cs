using System;

namespace Common.Interfaces
{
    public interface IUpdateAudit : IAudit
    {
        DateTime? UpdatedDate { get; set; }
        string UpdatedBy { get; set; }
    }
}