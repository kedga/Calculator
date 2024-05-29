using Calculator.Calculate;
using Calculator.Calculator;
using Calculator.UI;
using Calculator.Utilities;

namespace Calculator;

class Program
{
	
	static void Main(string[] args)
	{
		var ui = new ConsoleUI();

		var options = RootOption.GetOptions();

		var rootOption = ui.MakeSelection<RootOption>(opt =>
		{
			opt.Options = options;
			opt.OptionNames = options.Select(o => o.Name).ToList();
			opt.Prompt = "Select one:";
		});

		ui.ClearOutput();

		if (rootOption == RootOption.RpnCalc)
		{
			var calculator = new ConsoleLikeRpnCalculator(ui);

			calculator.Run();
		}

		else if (rootOption == RootOption.SequenceCalc)
		{
			RunCalculateSequence.Run(ui);
		}
	}
}

public record RootOption(string Name)
{
	public static RootOption RpnCalc { get; } = new ("Reverse Polish notation calculator");
	public static RootOption SequenceCalc { get; } = new ("Forward sequence calculator");

	public static List<RootOption> GetOptions() => [RpnCalc, SequenceCalc];
}