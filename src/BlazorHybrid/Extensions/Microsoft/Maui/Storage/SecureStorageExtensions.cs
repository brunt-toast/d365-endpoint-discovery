using System;
using System.Collections.Generic;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Extensions.Microsoft.Maui.Storage;

internal static class SecureStorageExtensions
{
    extension(ISecureStorage source)
    {
        public async Task<string> GetStringAsync(string key)
        {
            var ret = await source.GetAsync(key);
            return ret ?? string.Empty;
        }

        public async Task SetStringAsync(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                source.Remove(key);
            }
            else
            {
                await source.SetAsync(key, value);
            }
        }

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

        public async Task<T> GetEnumAsync<T>(string key) where T : struct, Enum
        {
            string? raw = await source.GetAsync(key);
            return Enum.TryParse<T>(raw, out var ret) 
                ? ret 
                : default;
        }

        public Task SetEnumAsync<T>(string key, T value) where T : struct, Enum
        {
            object obj = value;
            int i = (int)obj;
            return source.SetIntAsync(key, i);
        }
    }
}
