using Common;
using Common.Interfaces;
using System;

namespace Domain.Entities.Auth
{
    public class OTPVerification : BaseEntity<string>, ICreateAudit
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int OTP { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
