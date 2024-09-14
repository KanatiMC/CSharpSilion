using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace CSharpSilon.cmds
{
    public class Links: ModuleBase<SocketCommandContext>
    {
        public string[] FurryLinks = new string[]
        {
            "https://us.rule34.xxx//images/2271/273dd063efc905c2edf5b17b0e43045b.jpeg?11183340",
            "https://us.rule34.xxx//samples/2257/sample_c5abfbda663839a815da9ab72f2ac9ed.jpg?11183149",
            "https://us.rule34.xxx//images/2271/8b6c285279342a6576b9c33f95e56e5c.jpeg?11182120",
            "https://us.rule34.xxx//images/2271/85fac6d1bb1a6761e92fef49a3dca3d9.jpeg?11182111",
            "https://us.rule34.xxx//samples/2271/sample_44816f0608cb821e48c85a321008e082.jpg?11181928",
            "https://us.rule34.xxx//images/2271/91323363965b37fe6d145fad9b311a75.jpeg?11181042",
            "https://preview.redd.it/left-right-or-maybe-even-both-akibun-fm-v0-gaqr2ivpa0od1.jpeg?auto=webp&s=e7a7d49a26ac05b7e6d96d4e462b7d47482bb90a",
            "https://preview.redd.it/maybe-sugene-f-v0-hwcir0uzn2od1.jpeg?width=1080&crop=smart&auto=webp&s=41dc2008b58ddf3ebb19e14748808d0e6d417f2e",
            "https://preview.redd.it/hello-im-lilith-ready-to-have-some-fun-%D0%B7-millkydad-f-v0-2y1q5oo6m2od1.jpeg?width=1080&crop=smart&auto=webp&s=19ec6993f91e44d09791d8fe25a1f3efa52cdc85",
            "https://preview.redd.it/ych-irishshamrock-v2-chloe-dog-f-v0-eq0phhf5f3od1.png?auto=webp&s=17657f384a63d1c9f06c47c14bfe962907a6014a",
            "https://preview.redd.it/click-f-smileeeeeee-v0-86rnd8jibxnd1.jpeg?width=1080&crop=smart&auto=webp&s=59ae13696a24e5978c430da68807fd690ac0ca50",
            "https://preview.redd.it/dragon-boy-mm-gammainks-v0-9m4akhy5y1od1.jpeg?width=1080&crop=smart&auto=webp&s=005fe973965fb2ad7920b4db483146d60c0887c8",
            "https://preview.redd.it/lordy-leuuu-rabbitadvisory-f-v0-zkct7z8haznd1.jpeg?width=1080&crop=smart&auto=webp&s=d88117a38c6cba40646ae5783ea759bf7eb1b30c",
            "https://preview.redd.it/flexible-darkchibishadow-v0-l6kn9dio11od1.png?auto=webp&s=80bc49ab7f37c92bea15cfabc9219f90f628bc66",
            "https://preview.redd.it/even-dergs-need-to-get-off-regularly-f-inno-sjoa-v0-pxbhlou3bxnd1.png?width=1080&crop=smart&auto=webp&s=770ad4d494b09dd8d9d660a5fdec32e5660b46f8",
            "https://preview.redd.it/shes-good-at-grinding-mf-vksuika-v0-0di1foz3svld1.jpeg?auto=webp&s=ffd3ea5a43c4b6e45ccafd9a1005ebfef852cd45",
            "https://preview.redd.it/love-and-corruption-mf-tricking-him-a-little-bit-anshiin-v0-4ugr1x5aiild1.png?width=1080&crop=smart&auto=webp&s=945d47c99538d2d882fd5bee83fedd8430feaa2b",
            "https://media.discordapp.net/attachments/1187979000258244638/1261964089924522044/images.jpg?ex=66e1fee5&is=66e0ad65&hm=ff06af3e68fcb60dcf61e5223da434601ba27c50a081b19a687058181fcb6d7a&=&format=webp",
            "https://cdn.discordapp.com/attachments/1187979000258244638/1261963628182241290/Untitled-1.jpg?ex=66e1fe77&is=66e0acf7&hm=b9949dad32ae6fa9d59711721c529127d7c7e853b6e4cd1a0c75abd2a1a44881&",
            "https://cdn.discordapp.com/attachments/1187979000258244638/1261962102218690611/thumbnail_1ee39a2ac8e49ee13a95c17b63dd774e1196dfec.jpg?ex=66e1fd0b&is=66e0ab8b&hm=3e25eefd990cc4847297d7375f878498b058611e3147b83a0badf4e769f6d1a2&",
            "https://cdn.discordapp.com/attachments/1187979000258244638/1261962061169299536/images.jpg?ex=66e1fd01&is=66e0ab81&hm=ae73a3087b00642be2b4cda137f15e0d4837f5767ed1b75f636a7a8b79dad4ba&",
            "https://us.rule34.xxx/thumbnails/2889/thumbnail_eafc13b9f602697ede2d31e654de0969.jpg?10503841",
            "https://us.rule34.xxx/thumbnails/2378/thumbnail_0cf289464b98cefe1b5b4473cd0b3a4c.jpg?10509492",
            "https://cdn.discordapp.com/attachments/1187979000258244638/1261960201175826432/Untitled.jpg?ex=66e1fb46&is=66e0a9c6&hm=defac28da2ff8dc4852d1d3dd979df41ed981ae05c78a938363702a8751cf3cd&",
            "https://us.rule34.xxx/thumbnails/3592/thumbnail_bc3465717e8617e1b369b19ca99ee01a.jpg?4056524",
            "https://us.rule34.xxx/thumbnails/4714/thumbnail_fb11c78afb4277c0e1961fb383b6a17d.jpg?5369862",
            "https://us.rule34.xxx/thumbnails/5979/thumbnail_90d9d6e4c2c6804e28e7fbfb7af2d697.jpg?6800063",
            "https://us.rule34.xxx/thumbnails/5/thumbnail_c7bad498ced6131e184ec02f2762c3d0.jpg?10584353",
        };

        
        
        [Command("furry-invasion")]
        [Summary("Mass Opens Furry Porn On The User's Computer, 0 Will Do Infinite")]
        public async Task FurryInv(int amount = 0)
        {
            
            Random _random = new Random();
            if (amount == 0)
            {
                while (true)
                {
                    Utils.App.OpenLink(FurryLinks[_random.Next(0, FurryLinks.Length)]);
                }
            }
            else
            {
                for (int i = 0; i <= amount; i++)
                {
                    Utils.App.OpenLink(FurryLinks[_random.Next(0, FurryLinks.Length)]);
                }
            }
        }

        [Command("open-link"), Alias("openlink", "link")]
        public async Task OpenLink(string link)
        {
            Utils.App.OpenLink(link);
            await ReplyAsync($"Opened: {link}");
            
        }
        
        
        
        
    }
}