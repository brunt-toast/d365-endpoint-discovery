using System;
using System.Collections.Generic;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Extensions.Microsoft.Maui.Storage;

internal static class SecureStorageExtensions
{
    extension(ISecureStorage source)
    {
        public async Task<bool> GetBoolAsync(string key)
        {
            string? raw = await source.GetAsync(key);
            return key == "true";
        }

        public Task SetBoolAsync(string key, bool value)
        {
            return source.SetAsync(key, value ? "true" : "false");
        }

        public async Task<int> GetIntAsync(string key)
        {
            string? raw = await source.GetAsync(key);
            return int.TryParse(raw, out int ret) 
                ? ret 
                : 0;
        }

        public Task SetIntAsync(string key, int value)
        {
            return source.SetAsync(key, value.ToString());
        }
    }
}
