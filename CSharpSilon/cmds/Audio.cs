using System.IO;
using Discord.Commands;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using NAudio.Wave;

namespace CSharpSilon.cmds
{
    public class Audio : ModuleBase<SocketCommandContext>
    {
        private IWavePlayer waveOut;
        private AudioFileReader audioFile;
        
        public static CoreAudioDevice dpd;
        [Command("volume")]
        [Alias("vol")]
        public async Task SetVolume(int amnt)
        {
            if (amnt < 0) amnt = 0;
            if (amnt > 100) amnt = 100;
            if (dpd == null)
            {
                dpd = new CoreAudioController().DefaultPlaybackDevice;
            }

            dpd.Volume = amnt;

            await ReplyAsync($"Volume set to {amnt}%.");
        }
        

        [Command("play"), Summary("Plays The Audio File From A Specified Path")]
        public async Task Play(string filePath)
        {
            filePath = filePath.Replace("\"", "");
            if (File.Exists(filePath))
            {
                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                    audioFile.Dispose();
                }

                waveOut = new WaveOutEvent();
                audioFile = new AudioFileReader(filePath);
                waveOut.Init(audioFile);
                waveOut.Play();

                await ReplyAsync($"Playing: `{Path.GetFileName(filePath)}`");
            }
            else
            {
                await ReplyAsync("File Path Doesn't Exist.");
            }
        }
    }
}