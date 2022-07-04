using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
namespace MyCalcTest
{

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var calc = new Calc();
            Calc.EnterNumber(1);
            Calc.ExecuteOperation(Calc.CalculatorOperationType.Addition);
            Calc.EnterNumber(3);
            Calc.Equal();
            Assert.AreEqual("4", Calc.InputBuffer);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var calc = new Calc();
            Calc.EnterNumber(99);
            Calc.EraseLast();
            Calc.ExecuteOperation(Calc.CalculatorOperationType.Multiplying);
            Calc.EnterNumber(8);
            Calc.Percent();
            Calc.Equal();
            Calc.ExecuteOperation(Calc.CalculatorOperationType.Addition);
            Calc.EnterNumber(1);
            Calc.EnterDot();
            Calc.EnterNumber(1);
            Calc.Equal();
            Assert.AreEqual("7,58", Calc.InputBuffer);
        }
    }
}