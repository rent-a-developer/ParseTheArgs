using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTheArgs
{
    internal class ConsoleHelper
    {
        public virtual Boolean IsConsolePresent()
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

        public virtual TextWriter GetConsoleOutWriter()
        {
            return Console.Out;
        }

        public virtual TextWriter GetConsoleErrorWriter()
        {
            return Console.Error;
        }

        public virtual Int32 GetConsoleWindowWidth()
        {
            return Console.WindowWidth;
        }
    }
}
