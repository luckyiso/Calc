using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class MyCalc
    {
        public double Accumulator;
        public double Operand;
        public enum State { FirstOp, SecondOp };
        public State state = State.FirstOp;
        public string NumberText { get; private set; }
    }
}