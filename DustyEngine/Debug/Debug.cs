using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DustyEngine_V3
{
    public static class Debug
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error,
            FatalError,
        }

        private static List<string> logMessages = new List<string>();
        private static LogLevel currentLogLevel = LogLevel.Info;
        private static string logFilePath = "debug.log";
        private static bool writeToConsole = true;
        private static bool writeToFile = true;
        private static bool IsDebugMode = false;

        public static void Log(object? message, LogLevel level = LogLevel.Info, bool isDebugMessage = false,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (level >= currentLogLevel)
            {
                string formattedMessage =
                    $"[{DateTime.Now:HH:mm:ss}] [{level}] ({Path.GetFileName(file)}:{line} in {caller}) {message}";
                logMessages.Add(formattedMessage);


                if (writeToFile)
                    File.AppendAllText(logFilePath, formattedMessage + Environment.NewLine);

                if (IsDebugMode == false && isDebugMessage == true) return;
                if (writeToConsole)
                {
                    Console.WriteLine(formattedMessage);
                    Console.Out.Flush();
                }
            }
        }

        public static void SetLogLevel(LogLevel level) => currentLogLevel = level;

        public static void EnableConsoleLogging(bool enabled) => writeToConsole = enabled;

        public static void EnableFileLogging(bool enabled) => writeToFile = enabled;


        public static void ShowLogs()
        {
            foreach (var msg in logMessages) Console.WriteLine(msg);
        }

        public static void ClearLogs()
        {
            logMessages.Clear();
            File.WriteAllText(logFilePath, string.Empty);
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (!condition)
            {
                Log($"ASSERT FAILED: {message}", LogLevel.Error, true, caller, file, line);
                throw new Exception($"ASSERT FAILED: {message}");
            }
        }
    }
}