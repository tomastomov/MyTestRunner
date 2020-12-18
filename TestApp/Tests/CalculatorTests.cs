using CustomTestRunner;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        private IList<int> tempData;

        [TestSetup]
        public void Initialize()
        {
            tempData = new List<int>();
        }

        [TestMethod]
        public void AddShouldReturnCorrectOutput()
        {
            var calculator = new Calculator();

            var result = calculator.Add(13, 2);

            Assert.AreEqual(result, 15);

            tempData.Add(result);
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

        [TestMethod]
        public async Task AddAsyncShouldReturnCorrectOutput()
        {
            var calculator = new Calculator();

            await Task.Delay(5000).ConfigureAwait(false);

            var result = await calculator.AddAsync(3, 2).ConfigureAwait(false);

            Assert.AreEqual(result, 5);
        }

        [TestCleanup]
        public void DestroyData()
        {
            tempData = null;
        }
    }
}
