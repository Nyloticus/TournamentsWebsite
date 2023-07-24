using Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace Infrastructure
{
    public class UrlHelper : IUrlHelper
    {
        private readonly IHttpContextAccessor _accessor;

        public UrlHelper(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public string GetCurrentUrl()
        {

            return $"{_accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host.Value}/";
        }
        public string GetCurrentUrl(string url)
        {

            return $"{GetCurrentUrl()}/{url}";
        }
        public Uri GenerateUrl(string folder, string file)
        {
            if (!String.IsNullOrEmpty(folder) && !String.IsNullOrEmpty(file))
            {
                Uri uri = new Uri($"{_accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host}/{folder}/{file}");
                return uri;
            }
            return null;
        }
    }
}
