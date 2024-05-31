using System.Reflection;

namespace Calculator.RpnCalculatorV2;

public record Operator(string Symbol, int RequiredOperands, Func<List<Operand>, double> Operation) : CalculatorItem
{
	public static Operator Add { get; } = new("+", 2, PerformAddition);
	public static Operator Subtract { get; } = new("-", 2, PerformSubtraction);
	public static Operator Divide { get; } = new("/", 2, PerformDivision);
	public static Operator Multiply { get; } = new("*", 2, PerformMultiplication);
	public static Operator Exponential { get; } = new("exp", 2, PerformExponential);
	public static Operator Sine { get; } = new("sin", 1, PerformSine);

	private static List<Operator> GetAllOperators() =>
		typeof(Operator)
			.GetProperties(BindingFlags.Static | BindingFlags.Public)
			.Where(p => p.PropertyType == typeof(Operator))
			.Select(p => p.GetValue(null))
			.Cast<Operator>()
			.ToList();

	public static Operator? TryGetOperator(string symbol) =>
		GetAllOperators().FirstOrDefault(s => s.Symbol == symbol);

	public override string ToString()
	{
		return Symbol;
	}

	private static double PerformAddition(List<Operand> operands)
	{
		return operands.Select(o => o.Value).Sum();
	}

	private static double PerformDivision(List<Operand> operands)
	{
		var result = operands.Select(o => o.Value).Aggregate((acc, number) => acc / number);
		if (double.IsInfinity(result)) throw new DivideByZeroException("Cannot divide by zero");
		return result;
	}

	private static double PerformMultiplication(List<Operand> operands)
	{
		return operands.Select(o => o.Value).Aggregate((acc, number) => acc * number);
	}

	private static double PerformSubtraction(List<Operand> operands)
	{
		return operands.Select(o => o.Value).Aggregate((acc, number) => acc - number);
	}

	private static double PerformExponential(List<Operand> operands)
	{
		return operands.Select(o => o.Value).Aggregate(Math.Pow);
	}
	private static double PerformSine(List<Operand> operands)
	{
		return Math.Sin(operands.First().Value);
	}
}