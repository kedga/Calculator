using System.Reflection;

namespace Calculator.RpnCalculatorV2;

public record Operator(string Symbol, int RequiredOperands, Func<List<Operand>, double> Operation) : CalculatorItem
{
	public static Operator Add { get; } = new("+", 2, (operands) =>
	{
		return operands.Select(o => o.Value).Sum();
	});

	public static Operator Subtract { get; } = new("-", 2, (operands) =>
	{
		return operands.Select(o => o.Value).Aggregate((acc, number) => acc - number);
	});

	public static Operator Divide { get; } = new("/", 2, (operands) =>
	{
		var result = operands.Select(o => o.Value).Aggregate((acc, number) => acc / number);
		if (double.IsInfinity(result)) throw new DivideByZeroException("Cannot divide by zero");
		return result;
	});

	public static Operator Multiply { get; } = new("*", 2, (operands) =>
	{
		return operands.Select(o => o.Value).Aggregate((acc, number) => acc * number);
	});

	public static Operator Exponential { get; } = new("exp", 2, (operands) =>
	{
		return operands.Select(selector: o => o.Value).Aggregate(Math.Pow);
	});

	public static Operator Sine { get; } = new("sin", 1, (operands) =>
	{
		return Math.Sin(operands.First().Value);
	});

	public static List<Operator> GetAllOperators() =>
		typeof(Operator)
			.GetProperties(BindingFlags.Static | BindingFlags.Public)
			.Where(p => p.PropertyType == typeof(Operator))
			.Select(p => p.GetValue(null))
			.Cast<Operator>()
			.ToList();

	public override string ToString()
	{
		return Symbol;
	}
}