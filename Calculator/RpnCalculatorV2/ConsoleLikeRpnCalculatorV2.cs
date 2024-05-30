
using Calculator.UI;

namespace Calculator.RpnCalculatorV2;

public class ConsoleLikeRpnCalculatorV2(IBasicIO io)
{
	protected readonly StringRpnCalcV2 _calculator = new(io);
	public void Run()
    {
        const string performOperationCmd = "p";
		const string clearIoCmd = "c";
		const string quitCmd = "q";
		const string removeItemCmd = "r";

		const string basePrompt = @$"
Commands:

number  : Add a number
+ - / * : Add an operator
{quitCmd}       : Quit
{clearIoCmd}       : Clear numbers
{removeItemCmd}       : Remove last item from sequence
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

			else if (input.Equals(removeItemCmd)) _calculator.RemoveLastItem();

			else if (input.Equals(quitCmd)) break;

            else _calculator.TryAddItem(input);
        }
    }
}
