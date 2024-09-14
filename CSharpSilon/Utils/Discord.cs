using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace CSharpSilon.Utils
{
    public class Discord
    {
        
        public static bool IsChannelInCategory(IMessageChannel channel, SocketCategoryChannel category)
        {
            if (channel is SocketTextChannel textChannel)
            {
                return textChannel.CategoryId == category.Id;
            }
            else if (channel is SocketVoiceChannel voiceChannel)
            {
                return voiceChannel.CategoryId == category.Id;
            }

            return false;
        }
        
        public static async Task<bool> IsValid(string token)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/v9/users/@me")
                {
                    Headers =
                    {
                        { "Authorization", $"Bot {token}" }
                    }
                };

                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
        }
    }
}