
using Calculator.UI;

namespace Calculator.Calculator;

public class ConsoleLikeRpnCalculator(IBasicIO ui) : StringRpnCalculator
{
	protected readonly IBasicIO _ui = ui;
	public void Run()
    {
        while (true)
        {
            if (StackCount < 1)
            {
                OutputMessage = "Commands: q c + - * / number";
				_ui.PushOutput(OutputMessage);
			}

            OutputMessage = PrintStackContents();
			_ui.PushOutput(OutputMessage);

            string input = _ui.GetInput();

            if (input.Length < 1) continue;

            if (TryAddValue(input)) continue;

            if (input.Equals("c", StringComparison.InvariantCultureIgnoreCase)) Clear();

            else if (input.Equals("q", StringComparison.InvariantCultureIgnoreCase)) break;

			else if (TryGetOperator(input) is Operator validOperator)
			{
				var isOperationSuccessful = TryPerformOperation(validOperator);

				if (isOperationSuccessful) continue;

				else
				{
					_ui.PushOutput(OutputMessage);
					continue;
				};
			}

			else
            {
                OutputMessage = "Illegal command, ignored";
                _ui.PushOutput(OutputMessage);
            }
        }
    }
}
