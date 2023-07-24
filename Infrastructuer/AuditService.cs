using Common.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Infrastructure
{
    public class AuditService : IAuditService
    {
        private readonly IHttpContextAccessor _httpContext;

        public AuditService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public T CreateEntity<T>(T entity)
        {
            if (entity is ICreateAudit model)
            {
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = _httpContext.HttpContext.User?.Identity?.Name ?? "Anonymous";
                return (T)model;
            }

            return entity;
        }

        public T UpdateEntity<T>(T entity)
        {
            if (entity is IUpdateAudit model)
            {
                model.UpdatedDate = DateTime.UtcNow;
                model.UpdatedBy = _httpContext.HttpContext.User?.Identity?.Name ?? "Anonymous";
                return (T)model;
            }

            return entity;
        }

        public T DeleteEntity<T>(T entity)
        {
            if (entity is IDeleteEntity model)
            {
                model.DeletedDate = DateTime.UtcNow;
                model.DeletedBy = _httpContext.HttpContext.User?.Identity?.Name ?? "Anonymous";
                model.IsDeleted = true;
                return (T)model;
            }

            return entity;
        }

        bool IsInRole(string role)
        {
            return _httpContext.HttpContext.User.IsInRole(role);
        }

        public string UserName => _httpContext?.HttpContext?.User?.Identity?.Name ?? "Anonymous";
        public string SessionId => _httpContext?.HttpContext?.Request?.Headers["sessionId"];

        public string UserType => _httpContext.HttpContext?.User?.FindFirstValue("userType") ?? string.Empty;
        public string UserId => _httpContext?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
        public string RequesterIp => _httpContext.HttpContext.Connection.RemoteIpAddress.ToString();

        public string Role => "Anonymous";
        public string TenantId => _httpContext.HttpContext?.User?.FindFirstValue("TenantId") ?? string.Empty;
        public bool IsTenantUser => !string.IsNullOrEmpty(TenantId);
    }
}