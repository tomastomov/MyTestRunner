using CustomTestRunner;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var testRunner = new TestRunner();

            Console.WriteLine("Running tests.....");

            var assembly = Assembly.GetExecutingAssembly();

            var results = testRunner.Run(assembly, (test) =>
            {
                Console.WriteLine($"Test finished: {test.ToString()}");
            }).GetAwaiter().GetResult();

            foreach (var name in results.Keys)
            {
                Console.WriteLine(name);
                var tests = results[name];

                foreach (var result in tests)
                {
                    if (result.Success)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(result.ToString());
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(result.Errors.Aggregate(new StringBuilder(result.ToString() + Environment.NewLine), (sb, curr) =>
                        {
                            sb.AppendLine(curr);

                            return sb;
                        }).ToString());
                    }

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
            }
        }
    }
}
