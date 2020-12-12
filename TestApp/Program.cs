using CustomTestRunner;
using System;
using System.Linq;
using System.Text;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var testRunner = new TestRunner();

            Console.WriteLine("Running tests.....");

            var results = testRunner.Run();

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

    public interface ICalculator<T>
    {
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);
    }

    public class Calculator : ICalculator<int>
    {
        public int Add(int a, int b)
        {
            if (a > b)
            {
                throw new Exception("Cannot sum add a to b when a is less than b");
            }
            else return a + b;
        }

        public int Divide(int a, int b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("B cannot be 0");
            }
            return a / b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }
    }

    [TestClass]
    public class CalculatorTests
    {  
        [TestMethod]
        public void AddShouldReturnCorrectOutput()
        {
            var calculator = new Calculator();

            var result = calculator.Add(13, 2);

            Assert.AreEqual(result, 15);
        }

        [TestMethod]
        public void AddShouldThrowException()
        {
            var calculator = new Calculator();

            Assert.ThrowsException<Exception>(() => calculator.Add(1, 2));
        }

        [TestMethod]
        public void DivideShouldReturnCorrectOutput()
        {
            var calculator = new Calculator();

            Assert.AreEqual(calculator.Divide(4, 2), 2);
        }

        [TestMethod]
        public void DivideShouldThrowException()
        {
            var calculator = new Calculator();

            Assert.ThrowsException<DivideByZeroException>(() => calculator.Divide(1, 0));
        }

        [TestMethod]
        public void SubtractShouldReturnCorrectOutput()
        {
            var calculator = new Calculator();

            Assert.AreEqual(calculator.Subtract(3, 4), -1);
        }

        [TestMethod]
        public void MultiplyShouldReturnCorrectOutput()
        {
            var calculator = new Calculator();

            Assert.AreEqual(calculator.Multiply(3, 4), 12);
        }
    }
}
