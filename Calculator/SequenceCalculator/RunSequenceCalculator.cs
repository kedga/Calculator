using Calculator.UI;
using Calculator.Utilities;
using System.Reflection;

namespace Calculator.Calculate;

public static class RunSequenceCalculator
{
	public static void Run(ICanMakeSelectionUI ui)
	{
		while (true)
		{
			var calculator = ui.MakeSelection<ISequenceCalculator>(opt =>
			{
				var (calculatorInstances, calculatorNames) = GetCalculators();
				opt.Prompt = "Select calculator:";
				opt.Options = calculatorInstances;
				opt.OptionNames = calculatorNames;
			});

			var numbers = new List<double>();

			while (true)
			{
				var breakCommand = "o";

				ui.Clear();
				ui.PushOutput("Currently using " + calculator.GetType().Name);
				ui.PushOutput("\nEnter number to add to sequence");
				ui.PushOutput($"'{breakCommand}' to select operation");
				ui.PushOutput($"Numbers added: [{numbers.PrintCollection()}]");

				var userInput = ui.GetInput();

				if (double.TryParse(userInput, out var doubleValue))
				{
					numbers.Add(doubleValue);
					continue;
				}

				else if (userInput == breakCommand)
				{
					break;
				}
			}

			var operations = calculator.GetOperations();

			var operation = ui.MakeSelection<MathOperation>(opt =>
			{
				opt.Options = operations;
				opt.OptionNames = operations.Select(o => o.Operation.Method.Name).ToList();
				opt.Prompt = "Select operation to perform on sequence: [" + numbers.PrintCollection() + "]\n";
			});

			var resultMessage = string.Empty;

			try
			{
				var result = operation.Operation([.. numbers]);

				resultMessage = numbers.PrintCollection($" {operation.Symbol} ") + " = " + result;
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

	private static (List<ISequenceCalculator> instances, List<string> names) GetCalculators()
	{
		List<Type> calculatorTypes =
			Assembly.GetExecutingAssembly()
					.GetTypes()
					.Where(t => typeof(ISequenceCalculator).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
					.ToList();

		var calculatorInstances = new List<ISequenceCalculator>();
		var calculatorNames = new List<string>();

		foreach (var calculatorType in calculatorTypes)
		{
			if (Activator.CreateInstance(calculatorType) is ISequenceCalculator validCalculator)
			{
				calculatorInstances.Add(validCalculator);
				calculatorNames.Add(calculatorType.Name);
			}
		}

		return (calculatorInstances, calculatorNames);
	}
}
