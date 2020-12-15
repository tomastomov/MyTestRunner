using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public interface ICalculator<T>
    {
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);

        Task<T> AddAsync(T a, T b);
    }
}
