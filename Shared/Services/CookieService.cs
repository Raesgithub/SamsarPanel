
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ICookieService
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<T?> GetAsync<T>(string key);
        Task RemoveAsync(string key);
    }

    public class CookieService : ICookieService
    {
        private readonly IJSRuntime _jsRuntime;
        private const int DefaultExpirationDays = 30;

        public CookieService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var expires = expiration ?? TimeSpan.FromDays(DefaultExpirationDays);
            var expiresUtc = DateTime.UtcNow.Add(expires).ToString("R");

            var json = JsonSerializer.Serialize(value);
            var encodedValue = Uri.EscapeDataString(json);

            var cookie = $"{key}={encodedValue}; expires={expiresUtc}; path=/; samesite=strict";

            await _jsRuntime.InvokeVoidAsync("eval", $"""
            document.cookie = "{cookie}";
        """);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var js = $$"""
            function() {
                const value = document.cookie
                    .split('; ')
                    .find(row => row.startsWith('{{key}}='))
                    ?.split('=')[1];
                return value ? decodeURIComponent(value) : null;
            }
        """;

            var encodedValue = await _jsRuntime.InvokeAsync<string?>("eval", $"({js})()");

            if (string.IsNullOrEmpty(encodedValue))
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(encodedValue);
            }
            catch
            {
                return default;
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("eval", $"""
            document.cookie = "{key}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        """);
        }
    }
}
