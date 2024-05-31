using Calculator.Calculate;
using Calculator.Calculator;
using Calculator.RpnCalculatorV2;
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

		ui.Clear();

		if (rootOption == RootOption.RpnCalc)
		{
			var calculator = new ConsoleLikeRpnCalculator(ui);

			calculator.Run();
		}

		else if (rootOption == RootOption.RpnCalcV2)
		{
			var calculator = new ConsoleLikeRpnCalculatorV2(new RpnCalcV2(ui),ui);

			calculator.Run();
		}

		else if (rootOption == RootOption.SequenceCalc)
		{
			RunSequenceCalculator.Run(ui);
		}
	}
}

public record RootOption(string Name)
{
	public static RootOption RpnCalc { get; } = new ("Reverse Polish notation calculator");
	public static RootOption RpnCalcV2 { get; } = new("Reverse Polish notation calculator V2");
	public static RootOption SequenceCalc { get; } = new ("Non-reverse sequence calculator");

	public static List<RootOption> GetOptions() => [RpnCalcV2, RpnCalc, SequenceCalc];
}