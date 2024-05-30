﻿
using Calculator.UI;

namespace Calculator.RpnCalculatorV2;

public class ConsoleLikeRpnCalculatorV2(IBasicIO io, IRpnCalculatorV2 calculator)
{
	protected readonly IRpnCalculatorV2 _calculator = calculator;
	public void Run()
    {
        const string performOperationCmd = "p";
		const string clearIoCmd = "c";
		const string quitCmd = "q";

		const string basePrompt =
@$"Commands

number  : Add a number
+ - / * : Add an operator
{quitCmd}       : Quit
{clearIoCmd}       : Clear numbers
{performOperationCmd}       : Perform calculation
";

		while (true)
        {
            if (_calculator.ItemCount < 1)
            {
				io.PushOutput(basePrompt);
            }

			io.PushOutput(_calculator.PrintStackContents());

            string input = io.GetInput().ToLower();

            if (input.Length < 1) continue;

            else if (input.Equals(performOperationCmd)) _calculator.TryPerformOperation();

            else if (input.Equals(clearIoCmd)) _calculator.Clear();

            else if (input.Equals(quitCmd)) break;

            else _calculator.TryAddItem(input);
        }
    }
}