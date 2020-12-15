using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class Calculator : ICalculator<int>
    {
        public int Add(int a, int b)
        {
            if (a < b)
            {
                throw new Exception("Cannot sum add a to b when a is less than b");
            }
            else return a + b;
        }

        public Task<int> AddAsync(int a, int b)
        {
            return Task.Run(() => a + b);
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
}
