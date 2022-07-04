using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Calc
    {
        public enum CalculatorOperationType { Addition, Substraction, Dividing, Multiplying, None };
        public Calc()
        {
            InputBuffer = "0";
            OutputBuffer = string.Empty;
            InputRestriction = -1;

            isFirstNumValueSet = false;
            isBlocked = false;
            doesTheInputContainResOrPrevNum = false;
            doesTheInputContainConstantValue = false;
            isUnaryOperation = false;
            doesTheLastOutputContainsParentheses = false;
            MemorySymbolBool = false;


            firstNumValue = 0;
            secondNumValue = 0;
            unaryBuffer = string.Empty;
            lastComputation = string.Empty;

            nextOperationType = CalculatorOperationType.None;
            prevOperationType = CalculatorOperationType.None;

            decimalSeparator = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        }

        private static string _inputBuffer;
        private static string _outputBuffer;
        private static string unaryBuffer;
        private static string lastComputation;
        private static int _inputRestriction;

        private static bool isFirstNumValueSet;
        private static bool doesTheInputContainResOrPrevNum;
        private static bool doesTheInputContainConstantValue;
        private static bool isBlocked;
        private static bool isUnaryOperation;
        private static bool doesTheLastOutputContainsParentheses;
        public static bool MemorySymbolBool;

        private static double firstNumValue;
        private static double secondNumValue;
        private static double Mem = 0;

        public static event EventHandler InputBufferChanged;
        public static event EventHandler OutputBufferChanged;
        public static event EventHandler ComputationEnded;

        private static CalculatorOperationType nextOperationType;
        private static CalculatorOperationType prevOperationType;

        private static char decimalSeparator;
       
        public static string InputBuffer
        {
            get
            {
                return _inputBuffer;
            }
            private set
            {
                if (doesTheInputContainResOrPrevNum || doesTheInputContainConstantValue)
                {
                    if (_inputBuffer != string.Empty) value = value.Remove(0, _inputBuffer.Length);
                    if (value != string.Empty && value[0] == decimalSeparator) value = value.Insert(0, "0");
                    doesTheInputContainResOrPrevNum = false;
                    doesTheInputContainConstantValue = false;
                }

                if (value.Length > 1 && value[0] == '0')
                {
                    if (value[1] != decimalSeparator)
                        value = value.Substring(1);
                }
                _inputBuffer = value;
                OnInputBufferChanged(EventArgs.Empty);
            }
        }
        public static string OutputBuffer
        {
            get
            {
                if (_outputBuffer.Length >= 40) return "..." + _outputBuffer.Substring(_outputBuffer.Length - 40, 40);
                return _outputBuffer;
            }
            private set
            {
                _outputBuffer = value;
                OnOutputBufferChanged(EventArgs.Empty);
            }
        }
        public static int InputRestriction
        {
            get { return _inputRestriction; }
            set
            {
                if (value < -1) value = Math.Abs(value);
                _inputRestriction = value;
            }
        }
        public static void EnterNumber(int num)
        {
            if (isInputAllowed())
            {
                if (isUnaryOperation)
                {
                    EraseUnaryOperation();
                    InputBuffer = num.ToString();
                }
                else
                {
                    InputBuffer += num.ToString();
                }

            }
        }
        private static void AddNextOperation(in CalculatorOperationType operation, char sign)
        {
            double num;
            if (Double.TryParse(InputBuffer, out num))
            {
                if (!isFirstNumValueSet)
                {
                    firstNumValue = num;
                    isFirstNumValueSet = true;
                }
                else secondNumValue = num;

                string outputValueStr;

                if (nextOperationType != CalculatorOperationType.None && (!doesTheInputContainResOrPrevNum || doesTheInputContainConstantValue))
                {
                    ExecuteOperationImpl(nextOperationType);
                    if (isBlocked) return;

                    if (!isUnaryOperation)
                    {
                        outputValueStr = secondNumValue.ToString();
                    }
                    else
                    {
                        outputValueStr = string.Empty;
                        isUnaryOperation = false;
                        unaryBuffer = string.Empty;
                    }

                    if ((operation == CalculatorOperationType.Multiplying || operation == CalculatorOperationType.Dividing)
                        && (prevOperationType == CalculatorOperationType.Addition || prevOperationType == CalculatorOperationType.Substraction)
                        && !doesTheLastOutputContainsParentheses)
                    {
                        OutputBuffer = _outputBuffer + outputValueStr + ' ' + sign + ' ';
                        doesTheLastOutputContainsParentheses = true;
                    }
                    else OutputBuffer = _outputBuffer + outputValueStr + ' ' + sign + ' ';

                }
                else if (!doesTheInputContainResOrPrevNum || doesTheInputContainConstantValue)
                {
                    if (!isUnaryOperation) OutputBuffer = _outputBuffer + firstNumValue.ToString() + ' ' + sign + ' ';
                    else
                    {
                        OutputBuffer = _outputBuffer + " " + sign + " ";
                        isUnaryOperation = false;
                        unaryBuffer = string.Empty;
                    }

                    if (InputBuffer[InputBuffer.Length - 1] == decimalSeparator || (InputBuffer.Length > 1 && InputBuffer[InputBuffer.Length - 2] == decimalSeparator && InputBuffer[InputBuffer.Length - 1] == '0')) InputBuffer = firstNumValue.ToString();
                }
                else
                {
                    if (prevOperationType != CalculatorOperationType.None
                        && (operation == CalculatorOperationType.Substraction || operation == CalculatorOperationType.Addition)
                        && doesTheLastOutputContainsParentheses)
                    {
                        OutputBuffer = (_outputBuffer.Remove(_outputBuffer.Length - 4, 4) + ' ' + sign + ' ').Remove(0, 1);
                        doesTheLastOutputContainsParentheses = false;
                    }
                    else if (prevOperationType != CalculatorOperationType.None
                        && (operation == CalculatorOperationType.Multiplying || operation == CalculatorOperationType.Dividing)
                        && !doesTheLastOutputContainsParentheses)
                    {
                        OutputBuffer = _outputBuffer.Remove(_outputBuffer.Length - 3, 3) + ' ' + sign + ' ';
                        doesTheLastOutputContainsParentheses = true;
                    }
                    else
                    {
                        if (OutputBuffer == string.Empty && doesTheInputContainResOrPrevNum) OutputBuffer = firstNumValue.ToString() + " " + sign + " ";
                        else OutputBuffer = _outputBuffer.Remove(_outputBuffer.Length - 2, 2) + sign + ' ';
                    }
                }

                nextOperationType = operation;
                doesTheInputContainResOrPrevNum = true;
                doesTheInputContainConstantValue = false;


            }
        }
        public static void Clear()
        {

            doesTheInputContainResOrPrevNum = false;
            ClearOutputBuffer();

            isBlocked = false;
            InputBuffer = "0";


        }
        public static void ClearEntry()
        {
            doesTheInputContainConstantValue = false;
            doesTheInputContainResOrPrevNum = false;
            if (isUnaryOperation) EraseUnaryOperation();
            InputBuffer = "0";
            if (isBlocked)
            {
                OutputBuffer = string.Empty;
                unaryBuffer = string.Empty;
                isBlocked = false;
            }

        }
        public static void EraseLast()
        {
            if (!doesTheInputContainResOrPrevNum && !isBlocked && !doesTheInputContainConstantValue && !isUnaryOperation)
            {
                InputBuffer = InputBuffer.Remove(InputBuffer.Length - 1);
                if (InputBuffer.Length == 0 || (InputBuffer.Length == 1 && InputBuffer[0] == '-')) InputBuffer = "0";
            }
        }
        public static void Equal()
        {
            if (!isBlocked)
            {
                if (doesTheInputContainResOrPrevNum || (nextOperationType == CalculatorOperationType.None && isUnaryOperation))
                {

                    doesTheInputContainResOrPrevNum = true;

                    if (OutputBuffer != string.Empty)
                    {
                        if (isUnaryOperation)
                        {
                            lastComputation = _outputBuffer + " = " + InputBuffer;
                            ComputationEnded?.Invoke(null, new EventArgs());

                        }
                        else if (prevOperationType != CalculatorOperationType.None || _outputBuffer[_outputBuffer.Length - 4] == ')')
                        {
                            lastComputation = _outputBuffer.Remove(_outputBuffer.Length - 2, 2) + "= " + InputBuffer;
                            ComputationEnded?.Invoke(null, new EventArgs());
                        }


                    }
                    ClearOutputBuffer();

                }
                else if ((prevOperationType != CalculatorOperationType.None || (prevOperationType == CalculatorOperationType.None && nextOperationType != CalculatorOperationType.None)))
                {
                    if (Double.TryParse(InputBuffer, out secondNumValue))
                    {
                        string secondNum = InputBuffer;
                        ExecuteOperationImpl(nextOperationType);
                        if (!isUnaryOperation) lastComputation = _outputBuffer + secondNum + " = " + InputBuffer;
                        else lastComputation = _outputBuffer + " = " + InputBuffer;

                        ComputationEnded?.Invoke(null, new EventArgs());
                        ClearOutputBuffer();

                    }
                }
            }
        }
        public static void EnterDot()
        {
            if (isInputAllowed() && (!InputBuffer.Contains(decimalSeparator.ToString()) || doesTheInputContainResOrPrevNum || doesTheInputContainConstantValue))
            {
                if (isUnaryOperation)
                {
                    EraseUnaryOperation();
                    InputBuffer = "0" + decimalSeparator.ToString();
                }
                else InputBuffer += decimalSeparator;
            }
        }
        public static void SquareRoot()
        {
            if (!isBlocked)
            {
                double num;
                if (Double.TryParse(InputBuffer, out num))
                {
                    if (num >= 0)
                    {
                        PrintUnaryOperation(num, "sqrt");
                        InputBuffer = Math.Sqrt(num).ToString();
                    }
                    else
                    {
                        InvokeError();
                        return;
                    }
                }
            }
        }
        public static void ChangeSign()
        {

            if (!isBlocked)
            {
                double num;
                if (Double.TryParse(InputBuffer, out num))
                {
                    PrintUnaryOperation(num, "negate");
                    InputBuffer = (num * (-1)).ToString();

                }
            }
        }
        public static void ReverseFraction()
        {
            if (!isBlocked)
            {
                double num;
                if (Double.TryParse(InputBuffer, out num))
                {
                    if (num == 0)
                    {
                        InvokeError();
                        return;
                    }
                    PrintUnaryOperation(num, "reciproc");
                    InputBuffer = Math.Pow(num, -1).ToString();
                }
            }

        }
        public static void Percent()
        {
            if (!isBlocked)
            {
                double num;
                if (Double.TryParse(InputBuffer, out num) && isFirstNumValueSet == false)
                {
                    InputBuffer = (num * (0)).ToString();
                }
                else
                {
                    InputBuffer = (num / 100 * firstNumValue).ToString();
                }
            }
        }
        private static void ExecuteOperationImpl(CalculatorOperationType op)
        {
            switch (op)
            {
                case CalculatorOperationType.Addition:
                    firstNumValue += secondNumValue;
                    break;
                case CalculatorOperationType.Substraction:
                    firstNumValue -= secondNumValue;
                    break;
                case CalculatorOperationType.Dividing:
                    if (secondNumValue == 0)
                    {
                        InvokeError();
                        return;
                    }
                    else
                    {
                        firstNumValue /= secondNumValue;
                    }
                    break;
                case CalculatorOperationType.Multiplying:
                    firstNumValue *= secondNumValue;
                    break;
            }
            prevOperationType = nextOperationType;
            nextOperationType = CalculatorOperationType.None;
            unaryBuffer = string.Empty;
            doesTheInputContainConstantValue = false;
            InputBuffer = firstNumValue.ToString();
            doesTheInputContainResOrPrevNum = true;
            doesTheLastOutputContainsParentheses = false;

        }
        public static void ExecuteOperation(CalculatorOperationType op)
        {
            if (op != CalculatorOperationType.None && !isBlocked)
            {
                switch (op)
                {
                    case CalculatorOperationType.Addition:
                        AddNextOperation(CalculatorOperationType.Addition, '+');
                        break;
                    case CalculatorOperationType.Substraction:
                        AddNextOperation(CalculatorOperationType.Substraction, '-');
                        break;
                    case CalculatorOperationType.Dividing:
                        AddNextOperation(CalculatorOperationType.Dividing, '/');
                        break;
                    case CalculatorOperationType.Multiplying:
                        AddNextOperation(CalculatorOperationType.Multiplying, '*');
                        break;
                }
            }
        }
        private static void InvokeError()
        {
            prevOperationType = CalculatorOperationType.None;
            nextOperationType = CalculatorOperationType.None;
            doesTheInputContainResOrPrevNum = false;
            isFirstNumValueSet = false;
            isBlocked = true;
            InputBuffer = "Error";
        }
        protected static void OnInputBufferChanged(EventArgs e) => InputBufferChanged?.Invoke(null, e);
        protected static void OnOutputBufferChanged(EventArgs e) => OutputBufferChanged?.Invoke(null, e);
        private static bool isInputAllowed()
        {
            return (!isBlocked && (doesTheInputContainResOrPrevNum || (InputRestriction == -1 || InputBuffer.Length + 1 <= InputRestriction) || isUnaryOperation));
        }
        private static void EraseUnaryOperation()
        {
            if (OutputBuffer != string.Empty && nextOperationType != CalculatorOperationType.None)
            {

                if (unaryBuffer != string.Empty)
                {
                    OutputBuffer = _outputBuffer.Substring(0, _outputBuffer.Length - (unaryBuffer.Length + 1)) + ' ';
                }
            }
            else OutputBuffer = string.Empty;
            isUnaryOperation = false;
            unaryBuffer = string.Empty;
        }
        private static void ClearOutputBuffer()
        {
            isFirstNumValueSet = false;
            doesTheInputContainConstantValue = false;
            isUnaryOperation = false;
            OutputBuffer = string.Empty;
            unaryBuffer = string.Empty;
            doesTheLastOutputContainsParentheses = false;
            nextOperationType = CalculatorOperationType.None;
            prevOperationType = CalculatorOperationType.None;
        }
        private static void PrintUnaryOperation(double num, string functionName)
        {
            if (unaryBuffer != string.Empty)
            {
                string tempBuffer = unaryBuffer;
                EraseUnaryOperation();
                unaryBuffer = functionName + "(" + tempBuffer + ")";
            }
            else unaryBuffer = functionName + "(" + num.ToString() + ")";

            doesTheInputContainResOrPrevNum = false;
            isUnaryOperation = true;
            OutputBuffer = _outputBuffer + unaryBuffer;
        }
        public static void MS()
        {
            if (InputBuffer != "0")
            {
                Mem = Double.Parse(InputBuffer);
                MemorySymbolBool = true;
            }
            else
            {
                MemorySymbolBool = false;
                Mem = 0;
            }
        }
        public static void MC()
        {
            Mem = 0;
            MemorySymbolBool = false;
        }
        public static void MR()
        {
            InputBuffer = Mem.ToString();
            MemorySymbolBool = false;
        }
        public static void MPlus()
        {
            Mem = Mem + Double.Parse(InputBuffer);
        }
        public static void MMinus()
        {
            Mem = Mem - Double.Parse(InputBuffer);
        }

    }
}
