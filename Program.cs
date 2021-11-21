using System;
using System.Diagnostics;
using System.Reflection;

namespace SetPrompt
{
    internal class Program
    {
        public static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly string CurrentPrompt = Environment.GetEnvironmentVariable("PROMPT");
        public const string PROJECT_PAGE = @"https://github.com/Xapier14/SetPrompt";
        public static void Main(string[] args)
        {
            // GET CURRENT PROMPT
            string currentPrompt = Environment.GetEnvironmentVariable("PROMPT");

            // VERSION INFO
            Console.WriteLine($"SetPrompt v{Version.Major}.{Version.Minor}");
            Console.WriteLine($"GitHub: {PROJECT_PAGE}\n");

            // 'help' flag/switch
            if (HasSwitch(args, "?") ||
                HasFlag(args, "?") ||
                HasSwitch(args, "help") ||
                HasFlag(args, "help"))
            {
                Console.WriteLine("[Legend]");
                Console.WriteLine("&\t(Ampersand)");
                Console.WriteLine();
            }

            // whether to skip legend print
            if (!HasSwitch(args, "s") &&
                !HasFlag(args, "s"))
            {
                Console.WriteLine("[Legend]");
                Console.WriteLine("&\t(Ampersand)");
                Console.WriteLine("$B\t| (pipe)");
                Console.WriteLine("$C\t( (Left parenthesis)");
                Console.WriteLine("$E\tEscape code (ASCII code 27)");
                Console.WriteLine("$F\t) (Right parenthesis)");
                Console.WriteLine("$G\t> (greater-than sign)");
                Console.WriteLine("$H\tBackspace (erases previous character)");
                Console.WriteLine("$L\t< (less-than sign)");
                Console.WriteLine("$N\tCurrent drive");
                Console.WriteLine("$P\tCurrent drive and path");
                Console.WriteLine("$Q\t= (equal sign)");
                Console.WriteLine("$S\t  (space)");
                Console.WriteLine("$T\tCurrent time");
                Console.WriteLine("$V\tWindows version number");
                Console.WriteLine("$_\tCarriage return and linefeed");
                Console.WriteLine("$$\t$ (dollar sign)");
                Console.WriteLine("$+\tzero or more plus sign (+) characters depending upon the");
                Console.WriteLine("$\tdepth of the PUSHD directory stack, one character for each");
                Console.WriteLine("$\tlevel pushed.");
                Console.WriteLine("$M\tDisplays the remote name associated with the current drive");
                Console.WriteLine("$\tletter or the empty string if current drive is not a network");
                Console.WriteLine("$\tdrive.");
                Console.WriteLine();
            }

            // CURRENT PROMPT
            Console.WriteLine($"[CurrentPrompt]");
            Console.WriteLine($"Raw:\n{CurrentPrompt}\n");
            Console.WriteLine($"Parsed:\n{ParsePrompt(CurrentPrompt)}\n");

            // INPUT NEW PROMPT
            Console.Write("Set new prompt: ");
            string newPrompt = Console.ReadLine();

            Console.WriteLine($"\n[Preview]\n{ParsePrompt(newPrompt)}\n");

            // ASK IF CHANGE IS FOR ALL USERS
            Console.Write("Set for all users? (y/[n]): ");
            char yn = char.ToLower(Console.ReadKey().KeyChar);
            yn = (yn == 'y' || yn == 'n') ? yn : 'n';

            // REPEAT ANSWER
            Console.WriteLine();
            Console.WriteLine(yn == 'y' ? "Setting for all users..." :
                                          "Setting for current user...");
            // TRY CHANGE
            try
            {
                // start:
                // SETX PROMPT [/M] "<newPrompt>"
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "SETX",
                    Arguments = $"PROMPT {(yn == 'y' ? "/M " : "")} \"{newPrompt}\""
                }).WaitForExit();

                Console.WriteLine("Successfully set!");
            } catch
            {
                Console.WriteLine("Error setting new prompt.");
                Console.WriteLine("Try running in Administrator.");
            }
        }

        public static string ParsePrompt(string prompt)
        {
            return prompt.Replace("$A", "&", true, null)
                         .Replace("$B", "|", true, null)
                         .Replace("$C", "(", true, null)
                         .Replace("$D", DateTime.Now.ToLongDateString(), true, null)
                         .Replace("$E", "[ESCAPE]", true, null)
                         .Replace("$F", ")", true, null)
                         .Replace("$G", ">", true, null)
                         .Replace("$H", "[BACKSPACE]", true, null)
                         .Replace("$L", "<", true, null)
                         .Replace("$N", "[CurrentDrive]", true, null)
                         .Replace("$P", "[CurrentPath]", true, null)
                         .Replace("$Q", "=", true, null)
                         .Replace("$S", " ", true, null)
                         .Replace("$T", DateTime.Now.TimeOfDay.ToString(), true, null)
                         .Replace("$V", "[WindowsVersionNumber]", true, null)
                         .Replace("$_", "\n", true, null)
                         .Replace("$$", "$", true, null)
                         .Replace("$+", "[PUSHD Depth]", true, null)
                         .Replace("$M", "[RemotePath]", true, null);
        }

        public static bool HasFlag(string[] args, string flag)
        {
            foreach(string arg in args)
            {
                if (("-"+arg).ToLower() == flag.ToLower() ||
                    ("--"+arg).ToLower() == flag.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasSwitch(string[] args, string sw)
        {
            foreach (string arg in args)
            {
                if (("/" + arg).ToLower() == sw.ToLower() ||
                    ("\\" + arg).ToLower() == sw.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
