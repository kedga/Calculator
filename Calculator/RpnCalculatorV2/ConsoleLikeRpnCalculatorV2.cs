
using Calculator.UI;

namespace Calculator.RpnCalculatorV2;

public class ConsoleLikeRpnCalculatorV2(IRpnCalculator calculator, IBasicIO io)
{
	protected readonly IRpnCalculator _calculator = calculator;
	private readonly RpnCalcStringParser _parser = new();
	protected readonly IBasicIO _columnIO = new IOColumnsFormatter(io)
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
		while (true)
        {
            if (_calculator.ItemCount < 1)
            {
				PrintCommands();
            }

            string input = _columnIO.GetInput().ToLower();
			if (string.IsNullOrWhiteSpace(input)) continue;

			if (input.Equals(_performOperationCmd)) _calculator.TryPerformOperation();
			else if (input.Equals(_clearIoCmd)) _calculator.Clear();
			else if (input.Equals(_removeItemCmd)) _calculator.RemoveLastItem();
			else if (input.Equals(_quitCmd)) break;
			else _parser.TryAddItem(input, _calculator);
		}
	}

	private void PrintCommands()
	{
		_columnIO.PushOutput(["[ Commands ]"]);
		_columnIO.PushOutput(["number", "Add a number"]);
		_columnIO.PushOutput(["+ - / * exp sin", "Add an operator"]);
		_columnIO.PushOutput([_quitCmd, "Quit"]);
		_columnIO.PushOutput([_clearIoCmd, "Clear numbers"]);
		_columnIO.PushOutput([_performOperationCmd, "Perform calculation"]);
	}
}