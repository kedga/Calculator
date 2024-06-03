
using Calculator.UI;

namespace Calculator.RpnCalculatorV2;

public class ConsoleLikeRpnCalculatorV2(IRpnCalculator calculator, IBasicIO io)
{
	protected readonly IRpnCalculator _calculator = calculator;
	private readonly RpnCalcStringParser _parser = new();
	protected readonly ColumnsFormatterIO _columns = new(io)
	{
		StringFormat = ["{0, -20}", "{1, -20}"],
		Separator = " : "
	};

	const string _performOperationCmd = "p";
	const string _clearIoCmd = "c";
	const string _quitCmd = "q";
	const string _removeItemCmd = "r";

	public void Run()
    {
		PrintCommands();

		while (true)
        {
            string input = _columns.GetInput().ToLower();
			if (string.IsNullOrWhiteSpace(input))
			{
				PrintCommands();
				continue;
			}
			else if (input.Equals(_performOperationCmd))
			{
				_calculator.TryPerformOperation();
				continue;
			}
			else if (input.Equals(_clearIoCmd))
			{
				_calculator.Clear();
				continue;
			}
			else if (input.Equals(_removeItemCmd))
			{
				_calculator.RemoveLastItem();
				continue;
			}
			else if (input.Equals(_quitCmd)) break;
			else if (_parser.TryAddItem(input, _calculator)) continue;

			PrintCommands();
		}
	}

	private void PrintCommands()
	{
		_columns.PushOutput(["[ Commands ]"]);
		_columns.PushOutput(["number", "Add a number"]);
		_columns.PushOutput(["+ - / * exp sin", "Add an operator"]);
		_columns.PushOutput([_quitCmd, "Quit"]);
		_columns.PushOutput([_clearIoCmd, "Clear numbers"]);
		_columns.PushOutput([_performOperationCmd, "Perform calculation"]);
		_columns.PushOutput();
		_calculator.PrintStackContents();
	}
}