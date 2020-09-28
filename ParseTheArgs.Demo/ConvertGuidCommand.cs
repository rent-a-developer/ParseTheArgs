using System;
using System.Linq;
using System.Numerics;
using ParseTheArgs.Setup;

namespace ParseTheArgs.Demo
{
    public static class ConvertGuidCommand
    {
        public static Int32 ConvertGuid(ConvertGuidCommandArguments arguments)
        {
            try
            {
                var guidBytes = arguments.Guid.ToByteArray();

                switch (arguments.Mode)
                {
                    case ConvertGuidMode.Bytes:
                        var bytesString = String.Join(" ", guidBytes.Select(a => a.ToString()));

                        Console.WriteLine($"The Guid {arguments.Guid} converted to bytes is:");
                        Console.WriteLine(bytesString);

                        break;

                    case ConvertGuidMode.BigInteger:
                        var bigInteger = new BigInteger(guidBytes);

                        Console.WriteLine($"The Guid {arguments.Guid} converted to a big integer is:");
                        Console.WriteLine(bigInteger);

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception:");
                Console.Error.WriteLine(ex.ToString());
                return 1;
            }
        }

        public static void SetupCommand(ParserSetup parserSetup)
        {
            var command = parserSetup
                .Command<ConvertGuidCommandArguments>()
                .Name("convertGuid")
                .Help("Converts a guid.")
                .ExampleUsage("Toolbox convertGuid --guid ac23ddb2-6e34-46a9-80f4-9d6fe6f87558 --to Bytes");

            command
                .Argument(a => a.Guid)
                .Name("guid")
                .Help("The Guid to be converted.")
                .IsRequired();

            command
                .Argument(a => a.Mode)
                .Name("to")
                .Help("Defines to which the Guid should converted.")
                .OptionHelp(ConvertGuidMode.Bytes, "Converts the Guid to a sequence of bytes.")
                .OptionHelp(ConvertGuidMode.BigInteger, "Converts the Guid to a big integer.");
        }
    }
}
