using CuiLib;
using SCSearch.Commands;

namespace SCSearch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var command = new MainCommand();
            if (args.Length == 0)
            {
                command.WriteHelp(Console.Out);
                return;
            }

            try
            {
                command.Invoke(args);
            }
            catch (ArgumentAnalysisException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(e);
            }
        }
    }
}
