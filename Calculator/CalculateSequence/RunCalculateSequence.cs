using Calculator.UI;
using Calculator.Utilities;
using System.Reflection;

namespace Calculator.Calculate;

public static class RunCalculateSequence
{
	public static void Run(IUI ui)
	{
		while (true)
		{
			var calculator = ui.MakeSelection<ICalculateSequence>(opt =>
			{
				var (calculatorInstances, calculatorNames) = GetCalculators();
				opt.Prompt = "Select calculator:";
				opt.Options = [.. calculatorInstances];
				opt.OptionNames = [.. calculatorNames];
			});

			var numbers = new List<double>();

			while (true)
			{
				var breakCommand = "o";

				ui.ClearOutput();
				ui.PushOutput("Currently using " + calculator.GetType().Name);
				ui.PushOutput("\nAdd numbers to sequence!");
				ui.PushOutput($"'{breakCommand}' to select operation");
				ui.PushOutput($"Numbers added: [{numbers.PrintCollection()}]");

				var numberInput = ui.GetInput();

				if (double.TryParse(numberInput, out var doubleValue))
				{
					numbers.Add(doubleValue);
					continue;
				}

				else if (numberInput == breakCommand)
				{
					break;
				}
			}

			var operations = calculator.GetOperations();

			var prompt = "Select operation to perform on sequence: [" + numbers.PrintCollection() + "]\n";

			var operation = ui.MakeSelection<MathOperation>(opt =>
			{
				opt.Options = operations;
				opt.OptionNames = operations.Select(o => o.Operation.Method.Name).ToList();
				opt.Prompt = prompt;
			});

			var resultMessage = string.Empty;

			try
			{
				resultMessage = numbers.PrintCollection($" {operation.Symbol} ") + " = " + operation.Operation([.. numbers]);
			}
			catch (Exception ex)
			{
				resultMessage = "Unable to perform calculation: " + ex.Message;
			}

			resultMessage += "\n\nMake another calculation?\n";

			var doRestart = ui.AskYesOrNo(options =>
			{
				options.Prompt = resultMessage;
				options.YesOption = "Yes";
				options.NoOption = "No (exit application)";
			});

			if (doRestart) continue;

			else break;
		}
	}

	private static (List<ICalculateSequence> instances, List<string> names) GetCalculators()
	{
		List<Type> calculatorTypes =
			Assembly.GetExecutingAssembly()
					.GetTypes()
					.Where(t => typeof(ICalculateSequence).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
					.ToList();

		var calculatorInstances = new List<ICalculateSequence>();
		var calculatorNames = new List<string>();

		foreach (var calculatorType in calculatorTypes)
		{
			if (Activator.CreateInstance(calculatorType) is ICalculateSequence validCalculator)
			{
				calculatorInstances.Add(validCalculator);
				calculatorNames.Add(validCalculator.GetType().Name);
			}
		}

		return (calculatorInstances, calculatorNames);
	}
}
