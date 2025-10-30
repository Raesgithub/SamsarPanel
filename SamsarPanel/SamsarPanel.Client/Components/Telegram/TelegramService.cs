namespace SamsarPanel.Client.Components.Telegram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class TelegramMessage
    {
        public long chat_id { get; set; }
        public string text { get; set; }
        public string parse_mode { get; set; } = "HTML";
    }

    public class TelegramResponse
    {
        public bool ok { get; set; }
        public string description { get; set; }
    }

    public class TelegramUser
    {
        public long ChatId { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
    }

    public class TelegramBotService
    {
        private readonly string _botToken;
        private readonly HttpClient _httpClient;

        public TelegramBotService(string botToken)
        {
            _botToken = botToken;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// ارسال پیام به تمام کاربران بات
        /// </summary>
        public async Task SendMessageToAllMembers(string message)
        {
            try
            {
                var users = await GetActiveUsers();
                
                if (!users.Any())
                {
                    Console.WriteLine("❌ هیچ کاربر فعالی پیدا نشد.");
                    return;
                }

                var sentChatIds = new HashSet<long>();
                int successCount = 0;

                foreach (var user in users)
                {
                    if (sentChatIds.Contains(user.ChatId)) continue;

                    var success = await SendMessageAsync(user.ChatId, message);
                    if (success)
                    {
                        sentChatIds.Add(user.ChatId);
                        successCount++;
                        Console.WriteLine($"✅ ارسال به {user.FirstName} (@{user.Username})");
                        
                        // تأخیر برای جلوگیری از محدودیت تلگرام
                        await Task.Delay(200);
                    }
                }

                Console.WriteLine($"📊 نتیجه: {successCount} از {users.Count} کاربر با موفقیت دریافت کردند");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطا در ارسال گروهی: {ex.Message}");
            }
        }

        /// <summary>
        /// ارسال پیام به کاربر خاص بر اساس یوزرنیم
        /// </summary>
        public async Task<bool> SendMessageByUsername(string username, string message)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("❌ یوزرنیم نمی‌تواند خالی باشد");
                return false;
            }

            try
            {
                var users = await GetActiveUsers();
                var targetUser = users.FirstOrDefault(u => 
                    u.Username?.Equals(username.TrimStart('@'), StringComparison.OrdinalIgnoreCase) == true);

                if (targetUser == null)
                {
                    Console.WriteLine($"❌ کاربر با یوزرنیم @{username} پیدا نشد");
                    return false;
                }

                var success = await SendMessageAsync(targetUser.ChatId, message);
                if (success)
                {
                    Console.WriteLine($"✅ پیام به @{username} ارسال شد");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطا در ارسال به @{username}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ارسال پیام به کاربر خاص بر اساس Chat ID
        /// </summary>
        public async Task<bool> SendMessageByChatId(long chatId, string message)
        {
            try
            {
                // بررسی وجود کاربر (اختیاری - برای لاگ بهتر)
                var users = await GetActiveUsers();
                var userExists = users.Any(u => u.ChatId == chatId);

                if (!userExists)
                {
                    Console.WriteLine($"⚠️ کاربر با Chat ID {chatId} در لیست فعال‌ها نیست");
                }

                var success = await SendMessageAsync(chatId, message);
                if (success)
                {
                    Console.WriteLine($"✅ پیام به Chat ID {chatId} ارسال شد");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطا در ارسال به Chat ID {chatId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// دریافت لیست تمام کاربران فعال
        /// </summary>
        private async Task<List<TelegramUser>> GetActiveUsers()
        {
            var users = new List<TelegramUser>();
            
            try
            {
                string url = $"https://api.telegram.org/bot{_botToken}/getUpdates";
                var response = await _httpClient.GetStringAsync(url);
                
                var jsonDoc = JsonDocument.Parse(response);
                
                if (jsonDoc.RootElement.GetProperty("ok").GetBoolean())
                {
                    var results = jsonDoc.RootElement.GetProperty("result");
                    var seenChatIds = new HashSet<long>();

                    foreach (var update in results.EnumerateArray())
                    {
                        if (update.TryGetProperty("message", out var message) &&
                            message.TryGetProperty("chat", out var chat))
                        {
                            var chatId = chat.GetProperty("id").GetInt64();
                            
                            // جلوگیری از duplicate
                            if (seenChatIds.Add(chatId))
                            {
                                var user = new TelegramUser
                                {
                                    ChatId = chatId,
                                    FirstName = chat.GetProperty("first_name").GetString(),
                                    Username = chat.TryGetProperty("username", out var un) ? 
                                              un.GetString() : "ندارد"
                                };
                                
                                users.Add(user);
                            }
                        }
                    }
                }

                Console.WriteLine($"🔍 {users.Count} کاربر فعال پیدا شد");
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطا در دریافت کاربران: {ex.Message}");
                return users;
            }
        }

        /// <summary>
        /// ارسال پیام به کاربر خاص
        /// </summary>
        private async Task<bool> SendMessageAsync(long chatId, string text, string parseMode = "HTML")
        {
            try
            {
                var message = new TelegramMessage
                {
                    chat_id = chatId,
                    text = text,
                    parse_mode = parseMode
                };

                var json = JsonSerializer.Serialize(message);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var telegramResponse = JsonSerializer.Deserialize<TelegramResponse>(responseContent);
                    
                    if (telegramResponse?.ok == true)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"❌ خطای API: {telegramResponse?.description}");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"❌ خطای HTTP {response.StatusCode}: {responseContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطا در ارسال پیام: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// دریافت اطلاعات بات
        /// </summary>
        public async Task<string> GetBotInfo()
        {
            try
            {
                string url = $"https://api.telegram.org/bot{_botToken}/getMe";
                var response = await _httpClient.GetStringAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                return $"❌ خطا در دریافت اطلاعات بات: {ex.Message}";
            }
        }
    }
}