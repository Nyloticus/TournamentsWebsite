using System;

namespace Common.Interfaces
{
    public interface ICreateAudit : IAudit
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
    }
}