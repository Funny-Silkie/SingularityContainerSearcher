using CuiLib;
using SCSearch.Commands;

namespace SCSearch
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var command = new MainCommand();
            if (args.Length == 0)
            {
                command.WriteHelp(Console.Out);
                return;
            }

            try
            {
                await command.InvokeAsync(args);
            }
            catch (ArgumentAnalysisException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                await Console.Error.WriteLineAsync(e.Message);
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                await Console.Error.WriteLineAsync(e.ToString());
                Console.ResetColor();
            }
        }
    }
}
