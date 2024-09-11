using System.Linq;
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
            else if (channel is SocketStageChannel stageChannel)
            {
                return stageChannel.CategoryId == category.Id;
            }

            return false;
        }
    }
}