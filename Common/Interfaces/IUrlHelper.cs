using System;

namespace Common.Interfaces
{
    public interface IUrlHelper
    {
        string GetCurrentUrl();
        string GetCurrentUrl(string url);
        Uri GenerateUrl(string folder, string file);
    }
}
