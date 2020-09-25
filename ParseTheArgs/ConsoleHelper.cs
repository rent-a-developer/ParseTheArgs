using System;
using System.IO;

namespace ParseTheArgs
{
    internal static class ConsoleHelper
    {
        internal static Boolean IsConsolePresent()
        {
            try
            {
#pragma warning disable S1481 // Unused local variables should be removed
                var windowHeight = Console.WindowHeight;
#pragma warning restore S1481 // Unused local variables should be removed
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}
