using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        public Calc calc = new Calc();
        public MainWindow()
        {
            InitializeComponent();
            Calc.InputRestriction = 16;
        }
        public void Number_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string cont = b.Content.ToString();
            Calc.EnterNumber(Int32.Parse(cont));
        }
        private void CE(object sender, RoutedEventArgs e)
        {
            Calc.ClearEntry();
        }
        private void C(object sender, RoutedEventArgs e)
        {
            Calc.Clear();
        }

        private void Button_Plus(object sender, RoutedEventArgs e)
        {
            Calc.ExecuteOperation(Calc.CalculatorOperationType.Addition);
        }

        private void Button_Minus(object sender, RoutedEventArgs e)
        {
            Calc.ExecuteOperation(Calc.CalculatorOperationType.Substraction);
        }
        private void Button_Multiply(object sender, RoutedEventArgs e)
        {
            Calc.ExecuteOperation(Calc.CalculatorOperationType.Multiplying);
        }
        private void Button_Divide(object sender, RoutedEventArgs e)
        {
            Calc.ExecuteOperation(Calc.CalculatorOperationType.Dividing);
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            Calc.EraseLast();
        }

        private void Button_Equalation(object sender, RoutedEventArgs e)
        {
            Calc.Equal();
        }
        private void Button_Dot(object sender, RoutedEventArgs e)
        {
            Calc.EnterDot();
        }
        private void Button_Sqrt(object sender, RoutedEventArgs e)
        {
            Calc.SquareRoot();
        }
        private void Button_PlusMinus(object sender, RoutedEventArgs e)
        {
            Calc.ChangeSign();
        }
        private void Button_1OnX (object sender, RoutedEventArgs e)
        {
            Calc.ReverseFraction();
        }
        private void Button_Percent(object sender, RoutedEventArgs e)
        {
            Calc.Percent();
        }
        private void Button_MC(object sender, RoutedEventArgs e)
        {
            Calc.MC();
            MemorySymbol.Text = "";
        }
        private void Button_MR(object sender, RoutedEventArgs e)
        {
            Calc.MR();   
        }
        private void Button_MS(object sender, RoutedEventArgs e)
        {
            Calc.MS();
            if (Calc.MemorySymbolBool) MemorySymbol.Text = "M";
            else MemorySymbol.Text = "";
        }
        private void Button_MPlus(object sender, RoutedEventArgs e)
        {
            Calc.MPlus();
        }
        private void Button_MMinus(object sender, RoutedEventArgs e)
        {
            Calc.MMinus();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D8:
                    if (Keyboard.IsKeyDown(Key.LeftShift)) Calc.ExecuteOperation(Calc.CalculatorOperationType.Multiplying);
                    else Calc.EnterNumber(8);
                    break;
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D9:
                    Calc.EnterNumber((int)e.Key - 34);
                    break;
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    if (Keyboard.IsKeyToggled(Key.NumLock)) Calc.EnterNumber((int)e.Key - 74);
                    break;
                case Key.OemMinus:
                    if (!Keyboard.IsKeyDown(Key.LeftShift)) Calc.ExecuteOperation(Calc.CalculatorOperationType.Substraction);
                    break;
                case Key.OemPlus:
                    if (Keyboard.IsKeyDown(Key.LeftShift)) Calc.ExecuteOperation(Calc.CalculatorOperationType.Addition);
                    else Calc.Equal();
                    break;
                case Key.Back:
                    Calc.EraseLast();
                    break;
                case Key.Delete:
                    Calc.ClearEntry();
                    break;
                case Key.Oem2:
                    if (InputLanguageManager.Current.CurrentInputLanguage.ThreeLetterISOLanguageName == "eng" && !Keyboard.IsKeyDown(Key.LeftShift)) Calc.ExecuteOperation(Calc.CalculatorOperationType.Dividing);
                    else if (InputLanguageManager.Current.CurrentInputLanguage.ThreeLetterISOLanguageName == "rus" && !Keyboard.IsKeyDown(Key.LeftShift)) Calc.EnterDot();
                    break;
                case Key.Oem5:
                    if (InputLanguageManager.Current.CurrentInputLanguage.ThreeLetterISOLanguageName == "rus" && Keyboard.IsKeyDown(Key.LeftShift)) Calc.ExecuteOperation(Calc.CalculatorOperationType.Dividing);
                    break;
                case Key.OemPeriod:
                    if (InputLanguageManager.Current.CurrentInputLanguage.ThreeLetterISOLanguageName == "eng" && !Keyboard.IsKeyDown(Key.LeftShift)) Calc.EnterDot();
                    break;
                case Key.Multiply:
                    if (Keyboard.IsKeyToggled(Key.NumLock)) Calc.ExecuteOperation(Calc.CalculatorOperationType.Multiplying);
                    break;
                case Key.Divide:
                    if (Keyboard.IsKeyToggled(Key.NumLock)) Calc.ExecuteOperation(Calc.CalculatorOperationType.Dividing);
                    break;
                case Key.Add:
                    if (Keyboard.IsKeyToggled(Key.NumLock)) Calc.ExecuteOperation(Calc.CalculatorOperationType.Addition);
                    break;
                case Key.Subtract:
                    if (Keyboard.IsKeyToggled(Key.NumLock)) Calc.ExecuteOperation(Calc.CalculatorOperationType.Substraction);
                    break;
                case Key.Decimal:
                    if (Keyboard.IsKeyToggled(Key.NumLock)) Calc.EnterDot();
                    break;
            }
        }
    }
}